using System;
using System.Collections.Generic;
using System.Linq;
using FineBot.Abstracts;

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

        public Fine IssueFine(Guid issuerId, string reason, Guid? seconderId = null)
        {
           var fine = new Fine
                       {
                           IssuerId = issuerId,
                           Reason = reason,
                           SeconderId = seconderId,
                           AwardedDate = DateTime.UtcNow
                       };

            this.Fines.Add(fine);

            return fine;
        }

        public void PayFines(int number, byte[] image)
        {
            var orderedFines = this.Fines.Where(x => x.Outstanding).OrderBy(x => x.AwardedDate).ToList();

            var limit = Math.Max(number, orderedFines.Count());

            for(int i = 0; i < limit; i++)
            {
                orderedFines[i].Pay(image);
            }
        }
    }
}
