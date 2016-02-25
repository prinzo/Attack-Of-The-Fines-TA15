using System;
using FineBot.API.Extensions;

namespace FineBot.API.Models
{
    public class EditedMessage
    {
        public string user { get; set; }
        public double ts { get; set; }

        public DateTime TimeStamp
        {
            get { return ts.ToLocalDateTime(); }
        }
    }
}
