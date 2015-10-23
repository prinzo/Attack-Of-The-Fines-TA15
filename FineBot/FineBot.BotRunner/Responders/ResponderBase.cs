using System;
using FineBot.API.SupportApi;
using FineBot.Common.Infrastructure;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class ResponderBase
    {
        private readonly ISupportApi supportApi;

        public ResponderBase(
            ISupportApi supportApi
            )
        {
            this.supportApi = supportApi;
        }

        protected virtual BotMessage GetErrorResponse(ValidationResult result)
        {
            return new BotMessage { Text = String.Format("There was a problem with your request: {0}", result.FullTrace) };
        }

        protected virtual BotMessage GetExceptionResponse(Exception exception)
        {
            this.supportApi.CreateSupportTicketOnTrello(new SupportTicketModel()
                                                {
                                                    Message = String.Format("Exception in Bot Runner: {0}", exception.Message),
                                                    Subject = "Exception in Bot Runner",
                                                    Status = 1,
                                                    Type = 1
                                                });

            return new BotMessage { Text = String.Format("An error ocurred while processing your request, a ticket has been logged for support.") };
        }
    }
}