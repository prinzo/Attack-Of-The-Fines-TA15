using System;
using System.Collections.Generic;
using System.Linq;
using FineBot.Abstracts;
using FineBot.Common.Enums;
using FineBot.Common.Infrastructure;
using FineBot.Infrastructure;
using MongoDB.Driver;

namespace FineBot.Entities
{
    public class User : GuidIdentifiedEntity
    {
        public User()
        {
            this.Fines = new List<Fine>();
        }

        private string image;

        public string EmailAddress { get; set; }
        public string SlackId { get; set; }
        public string DisplayName { get; set; }
        public string Image { 
            get {
                if (image == null) {
                    image = "../../../../content/defaultUser.png";
                }

                return image; 
            } 

            set {
                image = value;
            }
        }
        public List<Fine> Fines { get; set; }

        public Fine GetFineById(Guid id)
        {
            return this.Fines.FirstOrDefault(f => f.Id == id);
        }

        public Fine GetOldestPendingFine()
        {
            return this.Fines.OrderBy(f => f.AwardedDate).FirstOrDefault(f => f.Pending);
        }

        public Fine GetNewestPendingFine()
        {
            return this.Fines.OrderByDescending(x => x.AwardedDate).FirstOrDefault(x => x.Pending);
        }

        public Fine IssueFine(Guid issuerId, string reason, Guid? seconderId = null)
        {
           var fine = new Fine
                       {
                           IssuerId = issuerId,
                           Reason = reason,
                           SeconderId = seconderId,
                           AwardedDate = DateTime.UtcNow,
                           ModifiedDate = DateTime.UtcNow
                       };

            this.Fines.Add(fine);

            return fine;
        }

        public ValidationResult PayFines(Payment payment, int number)
        {

            var validationResult = new ValidationResult();

            validationResult.Append(this.CanPayFines(number));

            if(payment.PayerId == this.Id)
            {
                validationResult.AddMessage(Severity.Error, "You cannot pay a fine for yourself!");
            }

            if(validationResult.HasErrors) return validationResult;

            var orderedFines = this.Fines.Where(x => x.Outstanding).OrderBy(x => x.AwardedDate).ToList();

            bool hasPending = orderedFines.All(x => x.Pending);

            if (hasPending)
            {
                return validationResult.AddMessage(Severity.Error, "Payable fines need to be seconded first");
            }

            var limit = Math.Max(number, orderedFines.Count());

            for(int i = 0; i < limit; i++)
            {
                orderedFines[i].Pay(payment.Id);
            }

            return validationResult;
        }

        public ValidationResult CanPayFines(int number)
        {
            var canPayFines = new ValidationResult();

            if (!this.Fines.Any(x => x.Outstanding))
            {
                canPayFines.AddMessage(Severity.Error, "This user doesn't have any fines to pay!");
                return canPayFines;
            }

            if(this.Fines.Count(x => x.Outstanding) < number)
            {
                canPayFines.AddMessage(Severity.Warning, "This user doesn't have that many fines!");
            }

            return canPayFines;
        }
    }
}
