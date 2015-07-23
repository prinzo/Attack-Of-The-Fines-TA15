using System;

namespace FineBot.API.FinesApi
{
    public class FineModel
    {
        public Guid IssuerId { get; set; }

        public Guid SeconderId { get; set; }

        public string Reason { get; set; }

        public byte[] RedemptionImageBytes { get; set; }
    }
}