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
    public class YouTubeResponder : ResponderBase, IFineBotResponder
    {
        private readonly IUserApi userApi;
        private readonly IFineApi fineApi;

        public YouTubeResponder(
            IUserApi userApi, 
            IFineApi fineApi,
            ISupportApi supportApi,
            IReactionApi reactionApi,
            IChatApi chatApi)
            : base(supportApi, reactionApi, chatApi)
        {
            this.userApi = userApi;
            this.fineApi = fineApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded && context.Message.IsYouTubeLink();
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            try
            {
                var youtubeLinkList = context.Message.GetYouTubeLinkList();
                const string reason = "for sharing the following YouTube video --> ";

                var issuer = userApi.GetUserBySlackId(context.FormattedBotUserID());
                var recipient = userApi.GetUserBySlackId(context.Message.User.FormattedUserID);
                var seconderSlackId = context.GetSecondCousinSlackId();
                if (seconderSlackId.Equals("")) seconderSlackId = recipient.SlackId;
                var seconder = userApi.GetUserBySlackId(seconderSlackId);

                for (var i = 0; i < youtubeLinkList.Count; i++)
                {
                    fineApi.IssueAutoFine(issuer.Id, recipient.Id, seconder.Id, reason + youtubeLinkList[i]);
                }

                reactionApi.AddReaction(ConfigurationManager.AppSettings["BotKey"], "fine", context.Message.GetChannelId(), context.Message.GetTimeStamp());
                reactionApi.AddReaction(ConfigurationManager.AppSettings["SeconderBotKey"], "fine", context.Message.GetChannelId(), context.Message.GetTimeStamp());

                return new BotMessage { Text = "" };
            }
            catch (Exception ex)
            {
                return this.GetExceptionResponse(ex, context.Message);
            }
        }
    }
}
