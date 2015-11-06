using System;
using System.Collections.Generic;
using System.Linq;
using FineBot.Abstracts;
using FineBot.API.Mappers.Interfaces;
using FineBot.API.MemberInfo;
using FineBot.DataAccess.DataModels;
using FineBot.Entities;
using FineBot.Interfaces;
using FineBot.Specifications;

namespace FineBot.API.UsersApi
{
    public class UserApi : IUserApi
    {
        private readonly IRepository<User, UserDataModel, Guid> userRepository;
        private readonly IUserMapper userMapper;
        private readonly IMemberInfoApi memberInfoApi;
        private readonly IRepository<Payment, PaymentDataModel, Guid> paymentRepository;

        public UserApi(
            IRepository<User, UserDataModel, Guid> userRepository,
            IUserMapper userMapper,
            IMemberInfoApi memberInfoApi,
            IRepository<Payment, PaymentDataModel, Guid> paymentRepository
            )
        {
            this.userRepository = userRepository;
            this.userMapper = userMapper;
            this.memberInfoApi = memberInfoApi;
            this.paymentRepository = paymentRepository;
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

        public void UpdateUser(UserModel userModel)
        {
            var user = this.userRepository.Get(userModel.Id);
            user.DisplayName = userModel.DisplayName;
            this.userRepository.Save(user);
        }

        public void UpdateUserImage(Guid userId, string image)
        {
            var user = userRepository.Get(userId);
            user.Image = image;
            userRepository.Save(user);
        }

        public UserModel RegisterUserByEmail(string email)
        {
            User newUser = new User
            {
                EmailAddress = email
            };

            var user = this.userRepository.Save(newUser);

            return this.userMapper.MapToModelShallow(user);
        }

        public string[] GetUserNameAndSurnameFromEmail(string email)
        {
            var domainName = email.Split('@');
            var splitName = domainName[0].Split('.');
            var name = splitName[0];
            var surname = domainName[0].Contains(".") ? splitName[1] : "N/A";
            var nameAndSurname = new[]{name, surname};
            return nameAndSurname;
        }

        public int GetNumberOfFinesForUserById(Guid id)
        {
            var user = this.userRepository.Get(id);

            return user.Fines.Count;
        }

        public int GetOutstandingFineCountForUser(Guid userId)
        {
            var user = userRepository.Get(userId);
            return user.Fines.Count(x => x.Outstanding);
        }

        public UserModel RegisterUserBySlackId(string slackId)
        {
            var info = this.memberInfoApi.GetMemberInformation(slackId);

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
                            SlackId = slackId,
                            DisplayName = String.IsNullOrEmpty(info.name) ? String.Join(" ",this.GetUserNameAndSurnameFromEmail(info.profile.email)) : info.name
                        };

            var user = this.userRepository.Save(newUser);

            return this.userMapper.MapToModelShallow(user);
        }

      

        public List<UserModel> GetLeaderboard(int number)
        {
            return Enumerable.ToList<UserModel>(this.userRepository
                .GetAll()
                .OrderByDescending(x => x.Fines.Count)
                .Take(number)
                .Select(x => this.userMapper.MapToModelShallow(x)));
        }

        public List<UserModel> GetLeaderboardToday(int number)
        {
            return Enumerable.ToList<UserModel>(this.userRepository
                .GetAll()
                .Where(x => x.Fines.Any(c => c.AwardedDate >= DateTime.Today))
                .Take(number).Select(y => this.userMapper.MapToModelWithDate(y)));
  
        }

        public List<UserModel> GetLeaderboardForThisWeek(int number)
        {
            return Enumerable.ToList<UserModel>(this.userRepository
                .GetAll()
                .OrderByDescending(x => x.Fines.Count)
                .Take(number)
                .Select(x => this.userMapper.MapToModelForThisWeek(x)));
        }

        public List<UserModel> GetLeaderboardForThisMonth(int number)
        {
            return Enumerable.ToList<UserModel>(this.userRepository
                .GetAll()
                .OrderByDescending(x => x.Fines.Count)
                .Take(number)
                .Select(x => this.userMapper.MapToModelForThisMonth(x)));
        }

        public List<UserModel> GetLeaderboardForThisYear(int number)
        {
            return Enumerable.ToList<UserModel>(this.userRepository
                .GetAll()
                .OrderByDescending(x => x.Fines.Count)
                .Take(number)
                .Select(x => this.userMapper.MapToModelForThisYear(x)));
        } 

        public List<UserModel> GetUsersWithPendingFines()
        {
            var users = this.userRepository.FindAll(new Specification<User>(u => u.Fines.Any(f => f.SeconderId == null)));

            return Enumerable.ToList<UserModel>(users.Select(u => this.userMapper.MapToModel(u)));
        }

        public List<UserModel> GetAllUsers() {
            var users = this.userRepository.FindAll(new Specification<User>());

            return Enumerable.ToList<UserModel>(users.Select(u => this.userMapper.MapToModelSmall(u)));
        }

        public UserStatisticModel GetStatisticsForUser(Guid id) {
            var statistic = this.userRepository.FindAll(new UserSpecification().WithId(id))
                .Select(x => new UserStatisticModel {
                       TotalFinesEver = x.Fines.Count(y => !y.Pending),
                       TotalFinesForMonth = x.Fines.Count(y => y.AwardedDate.Month == DateTime.Now.Month && !y.Pending),
                       FinesMonthlyModels = x.Fines.Where(y => y.AwardedDate.Year == DateTime.Now.Year && !y.Pending)
                                                  .GroupBy(y => y.AwardedDate.Month)
                                                  .Select(y => new MonthlyGraphModel {
                                                      MonthIndex = y.Key,
                                                      Count = y.Count()
                                                  }).ToList(),
                       TotalPaymentsEver = x.Fines.Count(y => y.PaymentId != null &&
                                                            paymentRepository
                                                           .Find(new PaymentSpecification()
                                                           .ValidWithId(y.PaymentId.Value)).IsValid),
                       TotalPaymentsForMonth = x.Fines.Count(y => y.PaymentId != null &&
                                                            paymentRepository
                                                           .Find(new PaymentSpecification()
                                                           .WithId(y.PaymentId.Value))
                                                           .PaidDate.Month == DateTime.Now.Month
                                                           && paymentRepository
                                                           .Find(new PaymentSpecification()
                                                           .WithId(y.PaymentId.Value)).IsValid),
                       PaymentsMonthlyModels = x.Fines.Where(y => y.PaymentId != null
                                                        && paymentRepository
                                                        .Find(new PaymentSpecification()
                                                        .WithId(y.PaymentId.Value))
                                                        .PaidDate.Month == DateTime.Now.Month
                                                        && paymentRepository
                                                        .Find(new PaymentSpecification()
                                                        .WithId(y.PaymentId.Value)).IsValid)
                                                 .GroupBy(y => y.AwardedDate.Month)
                                                 .Select(y => new MonthlyGraphModel
                                                 {
                                                     MonthIndex = y.Key,
                                                     Count = y.Count()
                                                 }).ToList(),
                        UserDisplayName = x.DisplayName
                });

            return statistic.FirstOrDefault();
        }
    }
}