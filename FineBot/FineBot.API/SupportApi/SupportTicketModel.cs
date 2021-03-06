﻿using System;

namespace FineBot.API.SupportApi
{
    public class SupportTicketModel
    {
        public SupportTicketModel()
        {
            this.CreatedDate = DateTime.Now;
        }

        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public int Type { get; set; }
        public string TypeDescription { get; set; }
        public Guid UserId { get; set; }
    }
}
