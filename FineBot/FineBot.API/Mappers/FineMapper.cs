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
                       PaymentImageBytes = this.MapPaymentImage(fine.PaymentImage),
                       SeconderId = fine.SeconderId,
                       Pending = fine.Pending,
                       AwardedDate = fine.AwardedDate
                   };
        }

        private PaymentImageModel MapPaymentImage(PaymentImage paymentImage)
        {
            if(paymentImage == null)
            {
                return null;
            }

            return new PaymentImageModel
                   {
                       FileName = paymentImage.FileName,
                       ImageBytes = paymentImage.ImageBytes,
                       MimeType = paymentImage.MimeType
                   };
        }

        public FineWithUserModel MapToModelWithUser(Fine fine, UserModel shallowUserModel)
        {
            return new FineWithUserModel
            {
                IssuerId = fine.IssuerId,
                Reason = fine.Reason,
                PaymentImageBytes = this.MapPaymentImage(fine.PaymentImage),
                SeconderId = fine.SeconderId,
                Pending = fine.Pending,
                AwardedDate = fine.AwardedDate,
                User = shallowUserModel
            };
        }

        public FeedFineModel MapToFeedModel(Fine fine, User issuer, User receiver) {
            return new FeedFineModel {
                IssuerId = fine.IssuerId,
                Reason = fine.Reason,
                SeconderId = fine.SeconderId,
                Pending = fine.Pending,
                AwardedDate = fine.AwardedDate,
                IssuerDisplayName = issuer.DisplayName,
                ReceiverDisplayName = receiver.DisplayName
            };
        }
    }
}