

using System;
using FineBot.Entities;
using FineBot.Abstracts;

namespace FineBot.DataAccess.DataModels {
    public class PaymentDataModel : GuidIdentifiedEntity {

        public DateTime PaidDate { get; set; }

        public PaymentImageDataModel PaymentImage { get; set; }

        public Guid PayerId { get; set; }
    }
}
