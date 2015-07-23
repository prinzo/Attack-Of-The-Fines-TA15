using System.Collections.Generic;
using FineBot.Abstracts;

namespace FineBot.Entities
{
    public class User : GuidIdentifiedEntity
    {
        public string EmailAddress { get; set; }
        public string SlackId { get; set; }

        public List<Fine> Fines { get; set; } 
    }
}
