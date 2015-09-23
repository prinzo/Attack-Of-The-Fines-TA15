using FineBot.DataAccess.BaseClasses;
using FineBot.DataAccess.DataModels;
using FineBot.DataAccess.Mappers.Interfaces;
using FineBot.Entities;
using MongoDB.Driver;

namespace FineBot.DataAccess.Mappers
{
    public class FineDataModelMapper : DataModelMapper<FineDataModel, Fine>
    {
        public override FineDataModel MapToModel(Fine fine)
        {
            return new FineDataModel
                   {
                       IssuerId = fine.IssuerId,
                       Reason = fine.Reason,
                       PaymentId = fine.PaymentId,
                       SeconderId = fine.SeconderId,
                       AwardedDate = fine.AwardedDate
                   };
        }

        public override Fine MapToDomain(FineDataModel model)
        {
            return new Fine
                   {
                       Id = model.Id,
                       AwardedDate = model.AwardedDate,
                       Reason = model.Reason,
                       PaymentId = model.PaymentId,
                       IssuerId = model.IssuerId,
                       SeconderId = model.SeconderId
                   };
        }

       
        private PaymentImageDataModel MapPaymentImageToModel(PaymentImage paymentImage)
        {
            if(paymentImage == null)
            {
                return null;
            }

            return new PaymentImageDataModel
                   {
                       FileName = paymentImage.FileName,
                       ImageBytes = paymentImage.ImageBytes,
                       MimeType = paymentImage.MimeType
                   };
        }

        private PaymentImage MapPaymentImageToDomain(PaymentImageDataModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new PaymentImage
            {
                FileName = model.FileName,
                ImageBytes = model.ImageBytes,
                MimeType = model.MimeType
            };
        }

        private PaymentDataModel MapPaymentToModel(Payment payment) {
            if (payment == null) {
                return null;
            }

            return new PaymentDataModel() {
                PayerId = payment.PayerId,
                PaymentImage = this.MapPaymentImageToModel(payment.PaymentImage),
                PaidDate = payment.PaidDate
            };
        }

        private Payment MapPaymentToDomain(PaymentDataModel payment) {
            if (payment == null) {
                return null;
            }

            return new Payment() {
                PayerId = payment.PayerId,
                PaymentImage = this.MapPaymentImageToDomain(payment.PaymentImage),
                PaidDate = payment.PaidDate
            };
        }
    }
}