

using System;
using FineBot.Entities;

namespace FineBot.DataAccess.DataModels {
    public class PaymentDataModel {
        public DateTime PaidDate { get; set; }

        public PaymentImageDataModel PaymentImage { get; set; }

        public Guid? PayerId { get; set; }
    }
}
