using System;
using System.Linq;
using FineBot.API.Mappers.Interfaces;
using FineBot.API.UsersApi;
using FineBot.Entities;

namespace FineBot.API.Mappers
{
    public class UserMapper : IUserMapper
    {
        private readonly IFineMapper fineMapper;

        public UserMapper(IFineMapper fineMapper)
        {
            this.fineMapper = fineMapper;
        }

        public UserModel MapToModelShallow(User user)
        {
            if(user == null) return null;

            return new UserModel
                   {
                       Id = user.Id,
                       SlackId = user.SlackId,
                       EmailAddress = user.EmailAddress,
                       AwardedFineCount = user.Fines.Count(x => !x.Pending),
                       PendingFineCount = user.Fines.Count(x => x.Pending)
                   };
        }

        public UserModel MapToModel(User user)
        {
            var userModel = this.MapToModelShallow(user);

            userModel.Fines = user.Fines.Select(x => this.fineMapper.MapToModel(x)).ToList();

            return userModel;
        }

        public UserModel MapToModelWithDate(User user, DateTime date)
        {
            if (user == null) return null;

            return new UserModel
            {
                Id = user.Id,
                SlackId = user.SlackId,
                EmailAddress = user.EmailAddress,
                AwardedFineCount = user.Fines.Count(x => x.AwardedDate.ToShortDateString() == DateTime.Today.ToShortDateString())
            };
        }
    }
}