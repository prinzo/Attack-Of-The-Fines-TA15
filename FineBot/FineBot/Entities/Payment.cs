using System;
using FineBot.Common.Enums;
using FineBot.Common.Infrastructure;
using Newtonsoft.Json.Bson;
using FineBot.Abstracts;
using System.Collections.Generic;
using FineBot.Enums;

namespace FineBot.Entities
{
    public class Payment : GuidIdentifiedEntity
    {
        public DateTime PaidDate { get; set; }

        public PaymentImage PaymentImage { get; set; }

        public Guid PayerId { get; set; }

        public List<Guid> LikedBy { get; set; }

        public List<Guid> DislikedBy { get; set; }

        public Payment(Guid payerId, byte[] image, string mimeType, string fileName)
        {
            this.PayerId = payerId;
            this.PaymentImage = new PaymentImage(image, mimeType, fileName);
            this.PaidDate = DateTime.Now;
        }

        public PlatformType Platform;

        public Payment() {}
    }
}