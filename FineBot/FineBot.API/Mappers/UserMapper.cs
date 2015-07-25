using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FineBot.API.Mappers.Interfaces;
using FineBot.API.UsersApi;
using FineBot.Entities;

namespace FineBot.API.Mappers
{
    public class UserMapper : IUserMapper
    {
        public UserModel MapToModel(User user)
        {
            if(user == null) return null;

            return new UserModel
                   {
                       Id = user.Id,
                       SlackId = user.SlackId,
                       EmailAddress = user.EmailAddress,
                       FineCount = user.Fines.Count
                   };
        }

        public UserModel MapToModelWithDate(User user, DateTime date)
        {
            if (user == null) return null;

            return new UserModel
            {
                Id = user.Id,
                SlackId = user.SlackId,
                EmailAddress = user.EmailAddress,
                FineCount = user.Fines.Count(x => x.AwardedDate.ToShortDateString() == DateTime.Today.ToShortDateString())
            };
        }
    }
}