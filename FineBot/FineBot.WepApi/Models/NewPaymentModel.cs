using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FineBot.WepApi.Models {
    public class NewPaymentModel {
        public DateTime PaidDate {
            get {
                return DateTime.Now;
            }
        }

        public byte[] Image { get; set; }

        public Guid PayerId { get; set; }

        public Guid RecipientId { get; set; }

        public int TotalFinesPaid { get; set; }
    }
}