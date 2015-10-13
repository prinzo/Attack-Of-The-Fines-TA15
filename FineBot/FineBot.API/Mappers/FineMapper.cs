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
            return this.MapToModel(fine, null);
        }

        public FineModel MapToModel(Fine fine, Payment payment)
        {
            return new FineModel
                   {
                       IssuerId = fine.IssuerId,
                       Reason = fine.Reason,
                       PaymentImageBytes = payment != null ? this.MapPaymentImage(payment.PaymentImage) : null,
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
            return this.MapToModelWithUser(fine, shallowUserModel, null);
        }

        public FineWithUserModel MapToModelWithUser(Fine fine, UserModel shallowUserModel, Payment payment)
        {
            return new FineWithUserModel
            {
                IssuerId = fine.IssuerId,
                Reason = fine.Reason,
                PaymentImageBytes = payment != null ? this.MapPaymentImage(payment.PaymentImage) : null,
                SeconderId = fine.SeconderId,
                Pending = fine.Pending,
                AwardedDate = fine.AwardedDate,
                User = shallowUserModel
            };
        }

        public FeedFineModel MapToFeedModel(Fine fine, User issuer, User receiver)
        {
            return this.MapToFeedModel(fine, issuer, receiver, null);
        }

        public FeedFineModel MapToFeedModel(Fine fine, User issuer, User receiver, Payment payment) {
            FeedFineModel fineModel = new FeedFineModel {
                Id = fine.Id,
                IssuerId = fine.IssuerId,
                Reason = fine.Reason,
                SeconderId = fine.SeconderId,
                Pending = fine.Pending,
                AwardedDate = fine.AwardedDate,
                IssuerDisplayName = issuer.DisplayName,
                ReceiverDisplayName = receiver.DisplayName,
                ModifiedDate = fine.ModifiedDate,
                UserImage = receiver.Image
            };
            
            if(payment != null)
            {
                fineModel.PaidDate = payment.PaidDate;
                fineModel.PayerId = payment.PayerId;
                fineModel.PaymentImage = payment.PaymentImage.ImageBytes.ToString();
            }

            return fineModel;
        }

        public FeedFineModel MapPaymentToFeedModel(Payment payment, User issuer, User receiver) {
            return new FeedFineModel {
                Id = payment.Id,
                IssuerDisplayName = issuer.DisplayName,
                ReceiverDisplayName = receiver.DisplayName,
                PaidDate = payment.PaidDate,
                ModifiedDate = payment.PaidDate,
                PayerId = payment.PayerId,
                PaymentImage = payment.PaymentImage != null && payment.PaymentImage.ImageBytes != null ? payment.PaymentImage.ImageBytes.ToString() : null,
                AwardedDate = payment.PaidDate,
                UserImage = receiver.Image
            };
        }

    }
}