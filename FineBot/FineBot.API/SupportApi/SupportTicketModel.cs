using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FineBot.API.SupportApi
{
    public class SupportTicketModel
    {
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public int Status { get; set; }

        public int Type { get; set; }

        public Guid UserId { get; set; }
    }
}
