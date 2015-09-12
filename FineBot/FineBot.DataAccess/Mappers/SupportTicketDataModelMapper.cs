using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FineBot.DataAccess.BaseClasses;
using FineBot.DataAccess.DataModels;
using FineBot.Entities;

namespace FineBot.DataAccess.Mappers
{
    public class SupportTicketDataModelMapper : DataModelMapper<SupportTicketDataModel, SupportTicket>
    {
        public SupportTicketDataModel MapToModel(SupportTicket supportTicket)
        {
            return new SupportTicketDataModel()
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

        public SupportTicket MapToDomain(SupportTicketDataModel supportTicketDataModel)
        {
            return new SupportTicket()
            {
                Id = supportTicketDataModel.Id,
                CreatedDate = supportTicketDataModel.CreatedDate,
                Subject = supportTicketDataModel.Subject,
                Message = supportTicketDataModel.Message,
                Status = supportTicketDataModel.Status,
                Type = supportTicketDataModel.Type,
                UserId = supportTicketDataModel.UserId
            };
        }
    }
}
