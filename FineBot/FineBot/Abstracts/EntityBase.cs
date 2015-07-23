using FineBot.Interfaces;

namespace FineBot.Abstracts
{
    public abstract class EntityBase<TIdentifier> : IEntity<TIdentifier>
    {
        public TIdentifier Id { get; set; }

        public EntityBase()
        {
            this.Id = default(TIdentifier);
        }
    }
}