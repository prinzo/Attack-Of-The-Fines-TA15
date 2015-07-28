using System;
using System.Collections.Generic;
using FineBot.API.FinesApi;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class GiveFineResponder : IGiveFineResponder
    {
        private readonly IUserApi userApi;
        private readonly IFineApi fineApi;

        public GiveFineResponder(
            IUserApi userApi,
            IFineApi fineApi
            )
        {
            this.userApi = userApi;
            this.fineApi = fineApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return context.Message.MentionsBot 
                && !context.BotHasResponded 
                && context.Message.Text.ToLower().Contains("fine");
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            List<string> userIds = context.Message.GetUserIdsFromMessageExcluding(context.BotUserID);

            if (userIds.Count == 0) return new BotMessage { Text = "I can't do that Dave" };

            var issuer = this.GetIssuer(context);

            if(issuer == null)
            {
                return new BotMessage{Text = String.Format("Sorry {0}, you may not issue fines until you have registered, say 'Register me' to me and you'll be in the system!", 
                    context.Message.User.FormattedUserID)};
            }

            string reason = this.GetReason(context);

            this.FineRecipients(userIds, issuer, reason);

            var botMessage = new BotMessage();

            string multiple = String.Empty;
            if(userIds.Count > 1)
            {
                multiple = "s";
            }

            botMessage.Text = String.Format("Fine{1} awarded to {0}! Somebody needs to second!", String.Join(", ", userIds), multiple);
            
            return botMessage;
        }

        private void FineRecipients(List<string> userIds, UserModel issuer, string reason)
        {
            foreach(var slackId in userIds)
            {
                var userModel = this.userApi.GetUserBySlackId(slackId);

                this.fineApi.IssueFine(issuer.Id, userModel.Id, reason);
            }
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