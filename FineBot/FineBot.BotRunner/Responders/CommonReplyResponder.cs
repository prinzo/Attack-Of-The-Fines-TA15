using System;
using System.Text;
using FineBot.API.FinesApi;
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
            ISupportApi supportApi
            ) : base (supportApi)
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

                var builder = new StringBuilder();
                builder.Append(context.FormattedBotUserID());
                builder.Append(": auto-fine ");
                builder.Append(context.Message.User.FormattedUserID);
                builder.Append(" ");
                builder.Append(reason);
                if(context.Message.Text.Contains("it works on my machine"))
                {
                    builder.Append(" :itworksonmymachine: ");
                }
                var issuer = userApi.GetUserBySlackId(context.FormattedBotUserID());
                var recipient = userApi.GetUserBySlackId(context.Message.User.FormattedUserID);
                var seconder = recipient;
                fineApi.IssueAutoFine(issuer.Id, recipient.Id, seconder.Id, reason);

                return new BotMessage { Text = builder.ToString() };
            }
            catch(Exception ex)
            {
                return this.GetExceptionResponse(ex);
            }
        }
    }
}
