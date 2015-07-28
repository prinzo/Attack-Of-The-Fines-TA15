using System;
using System.Collections.Generic;
using System.Linq;
using FineBot.Abstracts;
using FineBot.API.Mappers.Interfaces;
using FineBot.API.MemberInfo;
using FineBot.Entities;
using FineBot.Interfaces;
using FineBot.Specifications;

namespace FineBot.API.UsersApi
{
    public class UserApi : IUserApi
    {
        private readonly IRepository<User, Guid> userRepository;
        private readonly IUserMapper userMapper;
        private readonly IMemberInfoApi memberInfoApi;

        public UserApi(
            IRepository<User, Guid> userRepository,
            IUserMapper userMapper,
            IMemberInfoApi memberInfoApi
            )
        {
            this.userRepository = userRepository;
            this.userMapper = userMapper;
            this.memberInfoApi = memberInfoApi;
        }

        public UserModel GetUserById(Guid id)
        {
            var user = this.userRepository.Get(id);

            return this.userMapper.MapToModelShallow(user);
        }

        public UserModel GetUserBySlackId(string slackId)
        {
            var user = this.userRepository.Find(new UserSpecification().WithSlackId(slackId));

            if(user == null)
            {
                return this.RegisterUserBySlackId(slackId);
            }

            return this.userMapper.MapToModelShallow(user);
        }

        public UserModel GetUserByEmail(string email)
        {
            var user = this.userRepository.Find(new UserSpecification().WithEmailAddress(email));

            if (user == null)
            {
                return this.RegisterUserByEmail(email);
            }

            return this.userMapper.MapToModelShallow(user);
        }

        private UserModel RegisterUserByEmail(string email)
        {
            User newUser = new User
            {
                EmailAddress = email
            };

            var user = this.userRepository.Save(newUser);

            return this.userMapper.MapToModelShallow(user);
        }

        public int GetNumberOfFinesForUserById(Guid id)
        {
            var user = this.userRepository.Get(id);

            return user.Fines.Count;
        }

        public UserModel RegisterUserBySlackId(string slackId)
        {
            var cleanSlackId = slackId.StartsWith("<") ? slackId.Substring(2, slackId.Length - 3) : slackId;
            var info = this.memberInfoApi.GetMemberInformation(cleanSlackId);

            var foundUser = this.userRepository.Find(new UserSpecification().WithEmailAddress(info.profile.email));

            if(foundUser != null)
            {
                foundUser.SlackId = slackId;
                this.userRepository.Save(foundUser);

                return this.userMapper.MapToModelShallow(foundUser);
            }

            User newUser = new User
                        {
                            EmailAddress = info.profile.email,
                            SlackId = slackId
                        };

            var user = this.userRepository.Save(newUser);

            return this.userMapper.MapToModelShallow(user);
        }
        
        public List<UserModel> GetLeaderboard(int number)
        {
            return this.userRepository.GetAll().Take(number).OrderByDescending(x => x.Fines.Count).Select(x => this.userMapper.MapToModelShallow(x)).ToList();
        }

        public List<UserModel> GetLeaderboardToday(int number)
        {
            return this.userRepository.GetAll().Take(number).OrderByDescending(x => x.Fines.Count).Select(x => this.userMapper.MapToModelWithDate(x, DateTime.Today)).ToList();

        }

        public List<UserModel> GetUsersWithPendingFines()
        {
            var users = this.userRepository.FindAll(new Specification<User>(u => u.Fines.Any(f => f.SeconderId == null)));

            return users.Select(u => this.userMapper.MapToModel(u)).ToList();
        }
    }
}