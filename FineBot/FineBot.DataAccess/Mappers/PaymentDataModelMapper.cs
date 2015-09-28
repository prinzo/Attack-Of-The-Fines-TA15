using System;
using System.Linq;
using FineBot.DataAccess.BaseClasses;
using FineBot.DataAccess.DataModels;
using FineBot.DataAccess.Mappers.Interfaces;
using FineBot.Entities;

namespace FineBot.DataAccess.Mappers
{
    public class PaymentDataModelMapper : DataModelMapper<PaymentDataModel, Payment>
    {
        public override PaymentDataModel MapToModel(Payment payment) {
            return new PaymentDataModel {
               Id = payment.Id,
               PaidDate = payment.PaidDate,
               PayerId = payment.PayerId,
               PaymentImage = this.MapPaymentImageToModel(payment.PaymentImage)
            };
        }

        public override Payment MapToDomain(PaymentDataModel model) {
            return new Payment {
                Id = model.Id,
                PaidDate = model.PaidDate,
                PayerId = model.PayerId,
                PaymentImage = this.MapPaymentImageToDomain(model.PaymentImage)
            };
        }


        private PaymentImageDataModel MapPaymentImageToModel(PaymentImage paymentImage) {
            if (paymentImage == null) {
                return null;
            }

            return new PaymentImageDataModel {
                FileName = paymentImage.FileName,
                ImageBytes = paymentImage.ImageBytes,
                MimeType = paymentImage.MimeType
            };
        }

        private PaymentImage MapPaymentImageToDomain(PaymentImageDataModel model) {
            if (model == null) {
                return null;
            }

            return new PaymentImage {
                FileName = model.FileName,
                ImageBytes = model.ImageBytes,
                MimeType = model.MimeType
            };
        }

    }
}