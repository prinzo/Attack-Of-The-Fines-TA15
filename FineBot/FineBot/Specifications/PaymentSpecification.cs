using System;
using System.Linq;
using FineBot.Abstracts;
using FineBot.Entities;
using FineBot.Interfaces;

namespace FineBot.Specifications
{
    public class PaymentSpecification : Specification<Payment>
    {
        public ISpecification<Payment> WithId(Guid id)
        {
            return this.And(x => x.Id == id);
        }


        public ISpecification<Payment> ValidWithId(Guid id) {
            return this.And(x => x.Id == id);
        }
        
    }
}