using System;

namespace FineBot.API.FinesApi
{
    public class PaymentImageModel
    {
        public byte[] ImageBytes { get; set; }

        public string MimeType { get; set; }

        public string FileName { get; set; }
    }
}