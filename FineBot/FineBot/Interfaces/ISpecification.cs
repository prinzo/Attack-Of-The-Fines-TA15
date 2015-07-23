using System;
using System.Linq;
using System.Linq.Expressions;

namespace FineBot.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Predicate { get; }

        ISpecification<T> And(ISpecification<T> specification);

        ISpecification<T> And(Expression<Func<T, bool>> predicate);

        ISpecification<T> Or(ISpecification<T> specification);

        ISpecification<T> Or(Expression<Func<T, bool>> predicate);

        T SatisfyingItemFrom(IQueryable<T> query);

        IQueryable<T> SatisfyingItemsFrom(IQueryable<T> query);

    }
}