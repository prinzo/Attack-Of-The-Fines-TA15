using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FineBot.API.SupportApi;

namespace FineBot.WepApi.Controllers
{
    public class SupportController : ApiController
    {
        private readonly ISupportApi supportApi;

        public SupportController(ISupportApi supportApi)
        {
            this.supportApi = supportApi;
        }

        [HttpGet]
        public List<SupportTicketModel> GetAllSupportTickets()
        {
            return supportApi.GetAllSupportTickets();
        }

        [HttpGet]
        public List<SupportTicketModel> GetAllSupportTicketsForUser(Guid userId)
        {
            return supportApi.GetSupportTicketsForUser(userId);
        }
        [HttpPost]
        public string CreateSupportTicket(SupportTicketModel supportTicketModel)
        {
            try
            {
                var supportTicket = supportApi.CreateSupportTicket(supportTicketModel);
                supportApi.AddNewCardToSupport(supportTicket);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return "Support Ticket Created Successfully";
        }
    }
}