using System;
using FineBot.Abstracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace FineBot.DataAccess.DataModels
{
    public class FineDataModel : GuidIdentifiedEntity
    {
        public Guid IssuerId { get; set; }

        public Guid? SeconderId { get; set; }

        public DateTime AwardedDate { get; set; }

        public string Reason { get; set; }
        
        public Guid? PaymentId { get; set; }

        [BsonExtraElements]
        public BsonDocument CatchAll { get; set; }
    }
}