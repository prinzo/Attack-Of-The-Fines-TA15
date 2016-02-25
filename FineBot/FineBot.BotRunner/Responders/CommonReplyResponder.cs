using System;
using System.Configuration;
using FineBot.API.ChatApi;
using FineBot.API.FinesApi;
using FineBot.API.ReactionApi;
using FineBot.API.SupportApi;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class CommonReplyResponder : ResponderBase, IFineBotResponder
    {
        private readonly IUserApi userApi;
        private readonly IFineApi fineApi;

        public CommonReplyResponder(
            IUserApi userApi, 
            IFineApi fineApi,
            ISupportApi supportApi,
            IReactionApi reactionApi,
            IChatApi chatApi
            ) : base (supportApi, reactionApi, chatApi)
        {
            this.userApi = userApi;
            this.fineApi = fineApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded && context.Message.IsCommonReply();
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            try
            {
                const string reason = "for a common reply";

                if(context.Message.Text.Contains("it works on my machine"))
                {
                    reactionApi.AddReaction(ConfigurationManager.AppSettings["BotKey"], "itworksonmymachine", context.Message.GetChannelId(), context.Message.GetTimeStamp());
                }
                var issuer = userApi.GetUserBySlackId(context.FormattedBotUserID());
                var recipient = userApi.GetUserBySlackId(context.Message.User.FormattedUserID);
                var seconderSlackId = context.GetSecondCousinSlackId();
                if (seconderSlackId.Equals("")) seconderSlackId = recipient.SlackId;
                var seconder = userApi.GetUserBySlackId(seconderSlackId);

                fineApi.IssueAutoFine(issuer.Id, recipient.Id, seconder.Id, reason);

                reactionApi.AddReaction(ConfigurationManager.AppSettings["BotKey"], "fine", context.Message.GetChannelId(), context.Message.GetTimeStamp());
                reactionApi.AddReaction(ConfigurationManager.AppSettings["SeconderBotKey"], "fine", context.Message.GetChannelId(), context.Message.GetTimeStamp());

                return new BotMessage { Text = ""};
            }
            catch(Exception ex)
            {
                return this.GetExceptionResponse(ex, context.Message);
            }
        }
    }
}
