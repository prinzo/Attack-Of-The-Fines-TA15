using System;
using FineBot.API.Extensions;

namespace FineBot.API.Models
{
    public class Purpose
    {
        public string value { get; set; }
        public string creator { get; set; }
        public double last_set { get; set; }

        public DateTime LastSetDateTime
        {
            get { return last_set.ToLocalDateTime(); }
        }
    }
}
