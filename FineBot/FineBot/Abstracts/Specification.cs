using System;
using System.Linq;
using System.Linq.Expressions;
using FineBot.ExtensionMethods;
using FineBot.Interfaces;

namespace FineBot.Abstracts
{
    public class Specification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Predicate { get; protected set; }

        public Specification()
        {
            this.Predicate = T => true;
        }

        public Specification(Expression<Func<T, bool>> predicate)
        {
            this.Predicate = predicate;
        }

        public ISpecification<T> And(ISpecification<T> specification)
        {
            return new Specification<T>(this.Predicate.And(specification.Predicate));
        }

        public ISpecification<T> And(Expression<Func<T, bool>> predicate)
        {
            return new Specification<T>(this.Predicate.And(predicate));
        }

        public ISpecification<T> Or(ISpecification<T> specification)
        {
            return new Specification<T>(this.Predicate.Or(specification.Predicate));
        }

        public ISpecification<T> Or(Expression<Func<T, bool>> predicate)
        {
            return new Specification<T>(this.Predicate.Or(predicate));
        }

        public T SatisfyingItemFrom(IQueryable<T> query)
        {
            return query.Where(this.Predicate).SingleOrDefault();
        }

        public IQueryable<T> SatisfyingItemsFrom(IQueryable<T> query)
        {
            return query.Where(this.Predicate);
        }
    }
}