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

    }
}