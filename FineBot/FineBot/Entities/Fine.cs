using System;
using FineBot.Abstracts;

namespace FineBot.Entities
{
    public class Fine : GuidIdentifiedEntity
    {
        public Guid IssuerId { get; set; }

        public Guid? SeconderId { get; set; }

        public Guid? PayerId { get; set; }

        public bool Pending 
        {
            get
            {
                return this.SeconderId == null;
            }
        }

        public DateTime AwardedDate { get; set; }

        public string Reason { get; set; }

        public PaymentImage PaymentImage { get; set; }

        public bool Outstanding 
        {
            get
            {
                return this.PaymentImage == null;
            }
        }

        public void Second(Guid userId)
        {
            this.SeconderId = userId;
        }

        public void Pay(Guid payerId, byte[] image, string mimeType, string fileName)
        {
            this.PayerId = payerId;
            this.PaymentImage = new PaymentImage(image, mimeType, fileName);
        }
    }
}