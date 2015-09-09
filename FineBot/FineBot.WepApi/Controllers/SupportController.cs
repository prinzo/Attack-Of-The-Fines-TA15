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

        [HttpPost]
        public string CreateSupportTicket(SupportTicketModel supportTicketModel)
        {
            try
            {
                supportApi.CreateSupportTicket(supportTicketModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return "Support Ticket Created Successfully";
        }
    }
}