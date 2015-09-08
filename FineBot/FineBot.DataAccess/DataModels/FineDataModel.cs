using System;
using FineBot.Abstracts;

namespace FineBot.DataAccess.DataModels
{
    public class FineDataModel : GuidIdentifiedEntity
    {
        public Guid IssuerId { get; set; }

        public Guid? SeconderId { get; set; }

        public DateTime AwardedDate { get; set; }

        public string Reason { get; set; }

        public PaymentImageDataModel PaymentImageDataModel { get; set; }
    }
}