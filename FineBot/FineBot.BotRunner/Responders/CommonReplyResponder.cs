using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using FineBot.API.FinesApi;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class CommonReplyResponder : IFineBotResponder
    {
        private readonly IUserApi userApi;
        private readonly IFineApi fineApi;

        public CommonReplyResponder(IUserApi userApi, IFineApi fineApi)
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
            const string reason = "for a common reply";

            var builder = new StringBuilder();
            builder.Append(context.FormattedBotUserID());
            builder.Append(": auto-fine ");
            builder.Append(context.Message.User.FormattedUserID);
            builder.Append(" ");
            builder.Append(reason);
            if (context.Message.Text.Contains("it works on my machine"))
            {
                builder.Append(" :itworksonmymachine: ");
            }
            var issuer = userApi.GetUserBySlackId(context.FormattedBotUserID());
            var recipient = userApi.GetUserBySlackId(context.Message.User.FormattedUserID);
            var seconder = recipient;
            fineApi.IssueAutoFine(issuer.Id, recipient.Id, seconder.Id, reason);

            return new BotMessage { Text = builder.ToString() };
        }
    }
}
