using System.Collections.Generic;
using System.Linq;
using FineBot.API.Mappers.Interfaces;
using FineBot.API.SupportApi;
using FineBot.Common.ExtensionMethods;
using FineBot.Entities;
using FineBot.Enums;

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
                StatusDescription = ((Status)supportTicket.Status).ToDescription(),
                Type = supportTicket.Type,
                TypeDescription = ((SupportType)supportTicket.Type).ToDescription(),
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

        public List<SupportTicketModel> MapAllToModel(List<SupportTicket> supportTickets)
        {
            return supportTickets.Select(MapToModel).ToList();
        }
    }
}
