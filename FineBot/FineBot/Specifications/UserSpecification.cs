using System;
using FineBot.Abstracts;
using FineBot.Entities;
using FineBot.Interfaces;

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
    }
}