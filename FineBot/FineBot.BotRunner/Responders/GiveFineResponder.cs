using System;
using System.Collections.Generic;
using FineBot.API.FinesApi;
using FineBot.API.SupportApi;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using FineBot.Common.Infrastructure;
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
            ISupportApi supportApi
            )
            : base(supportApi)
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
                List<string> slackIds = context.Message.GetSlackIdsFromMessageExcluding(context.BotUserID);

                if (slackIds.Count == 0) return new BotMessage { Text = "I can't do that Dave" };

                var issuer = this.GetIssuer(context);

                string reason = this.GetReason(context);

                BotMessage botMessage = this.FineRecipients(slackIds, issuer, reason);

                return botMessage;
            } catch (Exception ex)
            {
                return this.GetExceptionResponse(ex);
            }
        }

        private BotMessage FineRecipients(List<string> userIds, UserModel issuer, string reason)
        {
            foreach(var slackId in userIds)
            {
                var userModel = this.userApi.GetUserBySlackId(slackId);

                IssueFineResult result = this.fineApi.IssueFine(issuer.Id, userModel.Id, reason);

                if (result.HasErrors) {
                    return this.GetErrorResponse(result);
                }
            }

            var botMessage = new BotMessage();

            string multiple = String.Empty;
            if (userIds.Count > 1) {
                multiple = "s";
            }

            botMessage.Text = String.Format("Fine{1} awarded to {0}! Somebody needs to second!", String.Join(", ", userIds), multiple);

            return botMessage;
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