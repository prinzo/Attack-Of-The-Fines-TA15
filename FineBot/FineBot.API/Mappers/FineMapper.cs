using FineBot.API.FinesApi;
using FineBot.API.Mappers.Interfaces;
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
                       RedemptionImageBytes = fine.RedemptionImageBytes,
                       SeconderId = fine.SeconderId
                   };
        }
    }
}