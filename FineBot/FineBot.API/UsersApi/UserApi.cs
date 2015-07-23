using System;
using FineBot.API.Mappers.Interfaces;
using FineBot.Entities;
using FineBot.Interfaces;
using FineBot.Specifications;

namespace FineBot.API.UsersApi
{
    public class UserApi : IUserApi
    {
        private readonly IRepository<User, Guid> userRepository;
        private readonly IUserMapper userMapper;

        public UserApi(
            IRepository<User, Guid> userRepository,
            IUserMapper userMapper
            )
        {
            this.userRepository = userRepository;
            this.userMapper = userMapper;
        }

        public UserModel GetUserById(Guid id)
        {
            var user = this.userRepository.Get(id);

            return this.userMapper.MapToModel(user);
        }

        public UserModel GetUserBySlackId(string slackId)
        {
            var user = this.userRepository.Find(new UserSpecification().WithSlackId(slackId));
            
            return this.userMapper.MapToModel(user);
        }

        public UserModel CreateUserFromSlackId(string slackUserId)
        {
            User newUser = new User
                           {
                               SlackId = slackUserId
                           };

            var user = this.userRepository.Save(newUser);

            return this.userMapper.MapToModel(user);
        }

        public int GetNumberOfFinesForUserById(Guid id)
        {
            var user = this.userRepository.Get(id);

            return user.Fines.Count;
        }
    }
}