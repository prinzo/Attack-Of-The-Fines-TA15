using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
    public class GiveFineResponder : ResponderBase, IFineBotResponder
    {
        private readonly IUserApi userApi;
        private readonly IFineApi fineApi;

        public GiveFineResponder(
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
            return context.Message.MentionsBot
                && !context.BotHasResponded
                && context.Message.StartsWithCommand(context.GetCommandMentioningBot("fine"));
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            try 
            {
                List<string> slackIds = context.Message.GetSlackIdsFromMessage();

                if (slackIds.Any(x => x.Equals(context.FormattedBotUserID())))
                {
                    reactionApi.AddReaction(ConfigurationManager.AppSettings["BotKey"], "middle_finger", context.Message.GetChannelId(), context.Message.GetTimeStamp());
                    return new BotMessage {Text = ""};
                }

                if (slackIds.Count == 0)
                {
                    reactionApi.AddReaction(ConfigurationManager.AppSettings["BotKey"], "raised_hand", context.Message.GetChannelId(), context.Message.GetTimeStamp());
                    return new BotMessage { Text = "" };
                }

                var issuer = this.GetIssuer(context);

                string reason = this.GetReason(context);

                BotMessage botMessage = this.FineRecipients(slackIds, issuer, reason, context.Message);

                return botMessage;
            } catch (Exception ex)
            {
                return this.GetExceptionResponse(ex, context.Message);
            }
        }

        private BotMessage FineRecipients(List<string> userIds, UserModel issuer, string reason, SlackMessage message)
        {
            foreach(var slackId in userIds)
            {
                var userModel = this.userApi.GetUserBySlackId(slackId);

                IssueFineResult result = this.fineApi.IssueFine(issuer.Id, userModel.Id, reason);

                if (result.HasErrors) {
                    return this.GetErrorResponse(result, message);
                }
            }

            reactionApi.AddReaction(ConfigurationManager.AppSettings["BotKey"], "ok_hand", message.GetChannelId(), message.GetTimeStamp());

            return new BotMessage{ Text = ""};
        }

        private UserModel GetIssuer(ResponseContext context)
        {
            var issuer = this.userApi.GetUserBySlackId(context.Message.User.FormattedUserID);

            return issuer;
        }

        private string GetReason(ResponseContext context)
        {
            var messageText = context.Message.Text;
            var startOfReason = messageText.IndexOf("for", StringComparison.Ordinal);
            if(startOfReason > 0)
            {
                return messageText.Substring(startOfReason);
            }

            return String.Empty;
        }
    }
}