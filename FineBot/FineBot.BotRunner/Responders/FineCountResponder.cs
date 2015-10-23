using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class FineCountResponder : IFineBotResponder
    {
        private readonly IUserApi userApi;

        public FineCountResponder(IUserApi userApi)
        {
            this.userApi = userApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded
                   && context.Message.MentionsBot
                   && context.Message.Text.ToLower().Contains("count");
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            var builder = new StringBuilder();
            builder.Append("Fine Count:\n");
            
            var slackUserIds = context.Message.GetUserIdsFromMessageExcluding(context.BotUserID);

            if (!slackUserIds.Any())
            {
                slackUserIds.Add(context.Message.User.FormattedUserID);
            }
            
            foreach (var slackUserId in slackUserIds)
            {
                var user = userApi.GetUserBySlackId(slackUserId);
                var userFineCount = userApi.GetOutstandingFineCountForUser(user.Id);

                builder.Append(slackUserId);
                builder.Append(" - ");
                builder.Append(userFineCount);
                builder.Append("\n");
            }
            
            return new BotMessage{ Text = builder.ToString() };
        }
    }
}
