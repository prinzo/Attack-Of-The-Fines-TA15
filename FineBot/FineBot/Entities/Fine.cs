using System;
using System.Drawing;
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

        public byte[] RedemptionImageBytes { get; set; }

        public Image RedemptionImage { get; set; }

        public void Second(Guid userId)
        {
            this.SeconderId = userId;
        }
    }
}