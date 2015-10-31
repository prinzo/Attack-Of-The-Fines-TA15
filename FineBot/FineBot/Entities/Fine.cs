using System;
using FineBot.Abstracts;
using FineBot.Enums;
using MongoDB.Driver;

namespace FineBot.Entities
{
    public class Fine : GuidIdentifiedEntity
    {
        public Guid IssuerId { get; set; }

        public Guid? SeconderId { get; set; }
        
        public bool Pending 
        {
            get
            {
                return this.SeconderId == null;
            }
        }
        
        public DateTime AwardedDate { get; set; }

        public Guid? PaymentId { get; set; }

        public string Reason { get; set; }
        
        public DateTime ModifiedDate { get; set; }

        public PlatformType Platform;

        public bool Outstanding {
            get {
                return this.PaymentId == null;
            }
        }

        public void Second(Guid userId)
        {
            this.SeconderId = userId;
        }

        public void Pay(Guid id)
        {
            this.PaymentId = id;
            this.ModifiedDate = DateTime.UtcNow;
        }
    }
}