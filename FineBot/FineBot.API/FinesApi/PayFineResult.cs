using System;
using FineBot.Common.Enums;
using FineBot.Common.Infrastructure;

namespace FineBot.API.FinesApi
{
    public class PayFineResult : ValidationResult
    {
        public PayFineResult(){}

        public PayFineResult(ValidationResult validation)
        {
            this.Append(validation);
        }

        public FeedFineModel FeedFineModel { get; set; }
    }
}