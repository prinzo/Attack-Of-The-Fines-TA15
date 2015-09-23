using System;
using FineBot.Common.Enums;
using FineBot.Common.Infrastructure;
using Newtonsoft.Json.Bson;

namespace FineBot.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }

        public DateTime PaidDate { get; set; }

        public PaymentImage PaymentImage { get; set; }

        public Guid PayerId { get; set; }

        public Payment(Guid payerId, byte[] image, string mimeType, string fileName)
        {
            this.PayerId = payerId;
            this.PaymentImage = new PaymentImage(image, mimeType, fileName);
            this.PaidDate = new DateTime();
        } 

        public Payment() {}

        public ValidationResult ValidatePaymentForUser(User user)
        {
            if (this.PayerId == this.Id) {
                return new ValidationResult().AddMessage(Severity.Error, "You cannot pay your own fines!");
            }

            return null;
        }
    }
}