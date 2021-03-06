﻿using System;
using System.Linq;
using FineBot.Abstracts;
using FineBot.Entities;
using FineBot.Interfaces;
using System.Collections.Generic;

namespace FineBot.Specifications
{
    public class UserSpecification : Specification<User>
    {
        public ISpecification<User> WithId(Guid id)
        {
            return this.And(x => x.Id == id);
        }

        public ISpecification<User> WithSlackId(string slackId)
        {
            return this.And(x => x.SlackId == slackId);
        }

        public ISpecification<User> WithEmailAddress(string email)
        {
            return this.And(x => x.EmailAddress == email);
        }

        public ISpecification<User> WithPendingFines()
        {
            return this.And(f => f.Fines.Any(x => x.SeconderId == null));
        }

        public ISpecification<User> WithFinesAwardedTodayBy(Guid issuerId) {
            DateTime todayStart = DateTime.Today.Date;
            DateTime todayEnd = DateTime.Today.Date.AddDays(1).AddSeconds(-1);

            return this.And(f => f.Fines.Any(x => x.IssuerId == issuerId && x.AwardedDate >= todayStart && x.AwardedDate < todayEnd));
        }

        public ISpecification<User> WithFineId(Guid id) {
            return this.And(x => x.Fines.Any(y => y.Id == id));
        }

        public ISpecification<User> WithIds(List<Guid> ids) {
            return this.And(x => ids.Contains(x.Id));
        }

    }
}