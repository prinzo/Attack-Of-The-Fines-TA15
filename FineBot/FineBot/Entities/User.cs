﻿using System;
using System.Collections.Generic;
using System.Linq;
using FineBot.Abstracts;
using FineBot.Common.Enums;
using FineBot.Common.Infrastructure;
using FineBot.Infrastructure;

namespace FineBot.Entities
{
    public class User : GuidIdentifiedEntity
    {
        public User()
        {
            this.Fines = new List<Fine>();
        }

        public string EmailAddress { get; set; }
        public string SlackId { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
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

        public ValidationResult PayFines(Guid payerId, int number, byte[] image, string mimeType, string fileName)
        {
            if(payerId == this.Id)
            {
                return new ValidationResult().AddMessage(Severity.Error, "You cannot pay your own fines!");
            }

            var orderedFines = this.Fines.Where(x => x.Outstanding).OrderBy(x => x.AwardedDate).ToList();

            var limit = Math.Max(number, orderedFines.Count());

            for(int i = 0; i < limit; i++)
            {
                orderedFines[i].Pay(payerId, image, mimeType, fileName);
                orderedFines[i].ModifiedDate = DateTime.UtcNow;
            }

            return new ValidationResult();
        }
    }
}
