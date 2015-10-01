using System;

namespace FineBot.API.FinesApi {
    public class PaymentModel {
        public byte[] Image { get; set; }

        public Guid PayerId { get; set; }

        public Guid RecipientId { get; set; }

        public int TotalFinesPaid { get; set; }

        public string ImageFileName { get; set; }
        public string MimeType { get; set; }
    }
}
