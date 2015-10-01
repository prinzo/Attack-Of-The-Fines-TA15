using FineBot.API.FinesApi;
using FineBot.API.Mappers.Interfaces;
using FineBot.Entities;

namespace FineBot.API.Mappers
{
    public class PaymentMapper : IPaymentMapper
    {
        public PaymentModel MapToSimpleModel(Payment payment)
        {
            return new PaymentModel
                   {
                       PayerId = payment.PayerId,
                       Image = payment.PaymentImage.ImageBytes,
                       ImageFileName = payment.PaymentImage.FileName,
                       MimeType = payment.PaymentImage.MimeType
                   };
        }
    }
}