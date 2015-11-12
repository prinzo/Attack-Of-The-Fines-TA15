using System;
using FineBot.Abstracts;
using FineBot.Common.Enums;
using FineBot.Common.Infrastructure;
using FineBot.Enums;

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

        public ValidationResult Second(Guid seconderId)
        {
            ValidationResult result = new ValidationResult();

            if(seconderId == IssuerId)
            {
                result.AddMessage(Severity.Information, "You may not second a fine you awarded!");
            }

            if(result.HasErrors) return result;

            this.SeconderId = seconderId;

            return result;
        }

        public void Pay(Guid id)
        {
            this.PaymentId = id;
            this.ModifiedDate = DateTime.UtcNow;
        }
    }
}