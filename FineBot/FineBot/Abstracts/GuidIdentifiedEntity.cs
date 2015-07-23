using System;

namespace FineBot.Abstracts
{
    public abstract class GuidIdentifiedEntity : EntityBase<Guid>
    {
        public GuidIdentifiedEntity()
        {
            this.Id = Guid.NewGuid();
        }
    }
}