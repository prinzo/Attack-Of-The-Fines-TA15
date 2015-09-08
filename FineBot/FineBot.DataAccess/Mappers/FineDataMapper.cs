using FineBot.DataAccess.DataModels;
using FineBot.DataAccess.Mappers.Interfaces;
using FineBot.Entities;

namespace FineBot.DataAccess.Mappers
{
    public class FineDataMapper : IFineDataMapper
    {
        public FineDataModel MapToModel(Fine fine)
        {
            return new FineDataModel
                   {
                       IssuerId = fine.IssuerId,
                       Reason = fine.Reason,
                       PaymentImageDataModel = this.MapPaymentImageToModel(fine.PaymentImage),
                       SeconderId = fine.SeconderId,
                       AwardedDate = fine.AwardedDate
                   };
        }

        public Fine MapToDomain(FineDataModel model)
        {
            return new Fine
                   {
                       Id = model.Id,
                       AwardedDate = model.AwardedDate,
                       Reason = model.Reason,
                       PaymentImage = this.MapPaymentImageToDomain(model.PaymentImageDataModel),
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
    }
}