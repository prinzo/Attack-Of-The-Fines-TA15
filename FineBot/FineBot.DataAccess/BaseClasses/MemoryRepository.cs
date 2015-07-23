using System.Collections.Generic;
using System.Linq;
using FineBot.Infrastructure;
using FineBot.Interfaces;

namespace FineBot.DataAccess.BaseClasses
{
    public class MemoryRepository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier> where TEntity : class, IEntity<TIdentifier>
    {
        private readonly Dictionary<TIdentifier, TEntity> memoryStore;

        public MemoryRepository()
        {
            this.memoryStore = new Dictionary<TIdentifier, TEntity>();
        }

        public virtual TEntity Get(TIdentifier id)
        {
            if(!this.memoryStore.ContainsKey(id)) return null;

            TEntity entity = this.memoryStore[id];

            return entity;
        }

        public virtual TEntity Find(ISpecification<TEntity> specification)
        {
            Guard.AgainstNull(() => specification);

            TEntity entity = this.memoryStore.Where(x => specification.Predicate.Compile().Invoke(x.Value)).Select(x => x.Value).SingleOrDefault();

            return entity;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            var entities = this.memoryStore.Values.ToList();

            return entities;
        }

        public virtual IEnumerable<TEntity> FindAll(ISpecification<TEntity> specification)
        {
            Guard.AgainstNull(() => specification);

            var entities = this.memoryStore.Where(x => specification.Predicate.Compile().Invoke(x.Value)).Select(x => x.Value);

            return entities;
        }

        public virtual TEntity Save(TEntity entity)
        {
            Guard.AgainstNull(() => entity);

            if(this.memoryStore.ContainsKey(entity.Id))
            {
                this.memoryStore[entity.Id] = entity;
            }
            else
            {
                this.memoryStore.Add(entity.Id, entity);                
            }

            return entity;
        }

        public virtual void Delete(TIdentifier id)
        {
            TEntity entity = this.Get(id);

            if (entity != null)
            {
                this.memoryStore.Remove(entity.Id);
            }
        }

        public virtual void Delete(TEntity entity)
        {
            Guard.AgainstNull(() => entity);

            this.Delete(entity.Id);
        }
    }
}