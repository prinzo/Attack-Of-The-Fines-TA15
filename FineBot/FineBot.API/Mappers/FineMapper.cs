using FineBot.API.FinesApi;
using FineBot.API.Mappers.Interfaces;
using FineBot.API.UsersApi;
using FineBot.Entities;

namespace FineBot.API.Mappers
{
    public class FineMapper : IFineMapper
    {
        public FineModel MapToModel(Fine fine)
        {
            return new FineModel
                   {
                       IssuerId = fine.IssuerId,
                       Reason = fine.Reason,
                       PaymentImageBytes = fine.PaymentImageBytes,
                       SeconderId = fine.SeconderId,
                       Pending = fine.Pending,
                       AwardedDate = fine.AwardedDate
                   };
        }

        public FineWithUserModel MapToModelWithUser(Fine fine, UserModel shallowUserModel)
        {
            return new FineWithUserModel
            {
                IssuerId = fine.IssuerId,
                Reason = fine.Reason,
                PaymentImageBytes = fine.PaymentImageBytes,
                SeconderId = fine.SeconderId,
                Pending = fine.Pending,
                AwardedDate = fine.AwardedDate,
                User = shallowUserModel
            };
        }
    }
}