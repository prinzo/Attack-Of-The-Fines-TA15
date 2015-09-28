using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FineBot.API.FinesApi {
    public class PaymentModel {
        public byte[] Image { get; set; }

        public Guid PayerId { get; set; }

        public Guid RecipientId { get; set; }

        public int TotalFinesPaid { get; set; }

        public DateTime DatePaid { get; set; }
    }
}
