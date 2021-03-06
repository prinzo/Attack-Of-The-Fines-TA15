﻿using FineBot.API.FinesApi;
using FineBot.API.Mappers.Interfaces;
using FineBot.API.UsersApi;
using FineBot.Entities;
using System.Text;
using FineBot.API.Extensions;
using FineBot.Common.ExtensionMethods;
using FineBot.Enums;

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

        public FeedFineModel MapToFeedModel(Fine fine, User issuer, User receiver, User seconder)
        {
            FeedFineModel model = this.MapToFeedModelWithPayment(fine, issuer, receiver, null);

            model.Seconder = seconder != null ? seconder.DisplayName : null;

            return model;
        }

        public FeedFineModel MapToFeedModelWithPayment(Fine fine, User issuer, User receiver, Payment payment) {
            FeedFineModel fineModel = new FeedFineModel {
                Id = fine.Id,
                IssuerId = fine.IssuerId,
                Reason = fine.Reason,
                SeconderId = fine.SeconderId,
                Pending = fine.Pending,
                AwardedDate = fine.AwardedDate,
                IssuerDisplayName = issuer.DisplayName,
                ReceiverDisplayName = receiver.DisplayName,
                ReceiverId = receiver.Id,
                ModifiedDate = fine.ModifiedDate,
                UserImage = receiver.Image,
                Platform = fine.Platform.GetDescription()
            };
            
            if(payment != null)
            {
                fineModel.PaidDate = payment.PaidDate;
                fineModel.PayerId = payment.PayerId;
                fineModel.PaymentImage = payment.PaymentImage.ImageBytes.ToString();
            }

            return fineModel;
        }

        public FeedFineModel MapPaymentToFeedModel(Payment payment, User issuer, User receiver, int totalPaid) {
            string paymentImage = payment.PaymentImage != null 
                                  && payment.PaymentImage.ImageBytes != null 
                                  && payment.PaymentImage.MimeType != null
                                  ? new StringBuilder("data:").Append(payment.PaymentImage.MimeType)
                                                     .Append(";base64,")
                                                     .Append(System.Convert.ToBase64String(payment.PaymentImage.ImageBytes))
                                                     .ToString()
                                  : null;

            return new FeedFineModel {
                Id = payment.Id,
                IssuerDisplayName = issuer.DisplayName,
                ReceiverDisplayName = receiver.DisplayName,
                PaidDate = payment.PaidDate,
                ModifiedDate = payment.PaidDate,
                PayerId = payment.PayerId,
                PaymentImage = paymentImage,
                AwardedDate = payment.PaidDate,
                UserImage = receiver.Image,
                TotalPaid = totalPaid,
                ReceiverId = receiver.Id,
                LikedBy = payment.LikedBy,
                DislikedBy = payment.DislikedBy,
                IssuerId = issuer.Id,
                Platform = payment.Platform.GetDescription()
            };
        }

    }
}