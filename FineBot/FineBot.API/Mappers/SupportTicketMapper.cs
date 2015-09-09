using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FineBot.API.Mappers.Interfaces;
using FineBot.API.SupportApi;
using FineBot.Entities;

namespace FineBot.API.Mappers
{
    public class SupportTicketMapper : ISupportTicketMapper
    {
        public SupportTicketModel MapToModel(SupportTicket supportTicket)
        {
            return new SupportTicketModel()
            {
                Id = supportTicket.Id,
                CreatedDate = supportTicket.CreatedDate,
                Subject = supportTicket.Subject,
                Message = supportTicket.Message,
                Status = supportTicket.Status,
                Type = supportTicket.Type,
                UserId = supportTicket.UserId
            };
        }

        public SupportTicket MapToDomain(SupportTicketModel supportTicketModel)
        {
            return new SupportTicket()
            {
                CreatedDate = supportTicketModel.CreatedDate,
                Subject = supportTicketModel.Subject,
                Message = supportTicketModel.Message,
                Status = supportTicketModel.Status,
                Type = supportTicketModel.Type,
                UserId = supportTicketModel.UserId
            };
        }
    }
}
