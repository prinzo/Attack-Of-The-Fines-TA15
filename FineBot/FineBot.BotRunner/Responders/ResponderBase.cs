using System;
using System.Configuration;
using FineBot.API.ReactionApi;
using FineBot.API.SupportApi;
using FineBot.BotRunner.Extensions;
using FineBot.Common.Infrastructure;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class ResponderBase
    {
        protected readonly ISupportApi supportApi;
        protected readonly IReactionApi reactionApi;

        public ResponderBase(
            ISupportApi supportApi, IReactionApi reactionApi)
        {
            this.supportApi = supportApi;
            this.reactionApi = reactionApi;
        }

        protected virtual BotMessage GetErrorResponse(ValidationResult result, SlackMessage message)
        {
            reactionApi.AddReaction(ConfigurationManager.AppSettings["BotKey"], "raised_hand", message.GetChannelId(), message.GetTimeStamp());
            return new BotMessage { Text = "" };
        }

        protected virtual BotMessage GetExceptionResponse(Exception exception, SlackMessage message)
        {
            this.supportApi.CreateSupportTicketOnTrello(new SupportTicketModel()
                                                {
                                                    Message = String.Format("Exception in Bot Runner: {0}", exception.Message),
                                                    Subject = "Exception in Bot Runner",
                                                    Status = 1,
                                                    Type = 1
                                                });

            reactionApi.AddReaction(ConfigurationManager.AppSettings["BotKey"], "exclamation", message.GetChannelId(), message.GetTimeStamp());
            return new BotMessage { Text = "" };
        }
    }
}