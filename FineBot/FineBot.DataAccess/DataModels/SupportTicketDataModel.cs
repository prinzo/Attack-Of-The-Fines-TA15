using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FineBot.Abstracts;

namespace FineBot.DataAccess.DataModels
{
    public class SupportTicketDataModel : GuidIdentifiedEntity
    {
        public DateTime CreatedDate { get; set; }

        public string Subject { get; set; }
        
        public string Message { get; set; }
        
        public int Status { get; set; }
        
        public int Type { get; set; }
        
        public Guid UserId { get; set; }
    }
}
