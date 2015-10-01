using FineBot.API.FinesApi;
using FineBot.Entities;

namespace FineBot.API.Mappers.Interfaces
{
    public interface IPaymentMapper
    {
        PaymentModel MapToSimpleModel(Payment payment);
    }
}