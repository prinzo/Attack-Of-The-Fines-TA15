using System.Collections.Generic;

namespace FineBot.Interfaces
{
    public interface IRepository<TEntity, in TIdentifier>
    {
        TEntity Get(TIdentifier id);

        TEntity Find(ISpecification<TEntity> specification);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> FindAll(ISpecification<TEntity> specification);

        TEntity Save(TEntity entity);

        void Delete(TIdentifier id);

        void Delete(TEntity entity);
    }
}