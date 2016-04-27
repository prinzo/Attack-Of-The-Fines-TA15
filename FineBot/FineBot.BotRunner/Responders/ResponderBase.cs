using System;
using System.Configuration;
using FineBot.API.ChatApi;
using FineBot.API.ReactionApi;
using FineBot.API.SupportApi;
using FineBot.BotRunner.Extensions;
using FineBot.Common.Infrastructure;
using MargieBot.Models;
using FineBot.Enums;

namespace FineBot.BotRunner.Responders
{
    public class ResponderBase
    {
        protected readonly ISupportApi supportApi;
        protected readonly IReactionApi reactionApi;
        protected readonly IChatApi chatApi;

        public ResponderBase(
            ISupportApi supportApi, IReactionApi reactionApi, IChatApi chatApi)
        {
            this.supportApi = supportApi;
            this.reactionApi = reactionApi;
            this.chatApi = chatApi;
        }

        protected virtual BotMessage GetErrorResponse(ValidationResult result, SlackMessage message)
        {
            reactionApi.AddReaction(ConfigurationManager.AppSettings["BotKey"], "raised_hand", message.GetChannelId(), message.GetTimeStamp());
            chatApi.PostMessage(ConfigurationManager.AppSettings["BotKey"], message.User.ID, result.FullTrace);
            return new BotMessage { Text = "" };
        }

        protected virtual BotMessage GetExceptionResponse(Exception exception, SlackMessage message)
        {
            this.supportApi.CreateSupportTicketOnTrello(new SupportTicketModel()
                                                {
                                                    Message = String.Format("Exception in Bot Runner: {0}", exception.Message),
                                                    Subject = "Exception in Bot Runner",
                                                    Status = (int) Status.Open,
                                                    Type = (int) SupportType.Bug
                                                });

            reactionApi.AddReaction(ConfigurationManager.AppSettings["BotKey"], "exclamation", message.GetChannelId(), message.GetTimeStamp());
            return new BotMessage { Text = "" };
        }
    }
}