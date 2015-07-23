﻿using System;
using System.Collections.Generic;
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

        public List<Fine> Fines { get; set; }

        public Fine IssueFine(Guid issuerId, string reason, Guid seconderId)
        {
           var fine = new Fine
                       {
                           IssuerId = issuerId,
                           Reason = reason,
                           SeconderId = seconderId
                       };

            this.Fines.Add(fine);

            return fine;
        }
    }
}