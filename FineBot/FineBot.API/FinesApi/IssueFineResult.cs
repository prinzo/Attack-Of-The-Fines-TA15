using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FineBot.Abstracts;
using FineBot.API.Mappers;
using FineBot.Common.Infrastructure;
using FineBot.Entities;

namespace FineBot.API.FinesApi {

    public class IssueFineResult : ValidationResult {

        public IssueFineResult(){}

        public IssueFineResult(ValidationResult validation)
        {
            this.Append(validation);
        }

        public FeedFineModel Fine { get; set; }

    }
}
