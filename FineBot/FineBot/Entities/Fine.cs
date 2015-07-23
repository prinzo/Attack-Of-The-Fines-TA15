using System;
using System.Drawing;

namespace FineBot.Entities
{
    public class Fine
    {
        public Guid IssuerId { get; set; }

        public Guid SeconderId { get; set; }

        public string Reason { get; set; }

        public byte[] RedemptionImageBytes { get; set; }

        public Image RedemptionImage { get; set; }
    }
}