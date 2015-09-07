using System;

namespace FineBot.API.FinesApi
{
    public class FineModel
    {
        public Guid IssuerId { get; set; }

        public Guid? SeconderId { get; set; }

        public bool Pending { get; set; }

        public string Reason { get; set; }

        public PaymentImageModel PaymentImageBytes { get; set; }

        public DateTime AwardedDate { get; set; }
    }
}