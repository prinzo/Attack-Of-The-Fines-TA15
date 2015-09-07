using System;
using FineBot.Abstracts;

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

        public string Reason { get; set; }

        public byte[] PaymentImageBytes { get; set; }

        public bool Outstanding 
        {
            get
            {
                return this.PaymentImageBytes == null;
            }
        }

        public void Second(Guid userId)
        {
            this.SeconderId = userId;
        }

        public void Pay(byte[] image)
        {
            this.PaymentImageBytes = image;
        }
    }
}