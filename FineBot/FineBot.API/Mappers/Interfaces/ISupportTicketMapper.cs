using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FineBot.API.SupportApi;
using FineBot.Entities;

namespace FineBot.API.Mappers.Interfaces
{
    public interface ISupportTicketMapper
    {
        SupportTicketModel MapToModel(SupportTicket supportTicket);
        SupportTicket MapToDomain(SupportTicketModel supportTicketModel);
        List<SupportTicketModel> MapAllToModel(List<SupportTicket> supportTickets);
    }
}
