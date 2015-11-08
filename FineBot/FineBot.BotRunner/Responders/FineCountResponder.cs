using System;
using System.Linq;
using System.Text;
using FineBot.API.SupportApi;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class FineCountResponder : ResponderBase, IFineBotResponder
    {
        private readonly IUserApi userApi;

        public FineCountResponder(
            IUserApi userApi,
            ISupportApi supportApi
            )
            : base(supportApi)
        {
            this.userApi = userApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded
                   && context.Message.MentionsBot
                   && context.Message.Text.ToLower().Contains("count")
                   && !context.Message.Text.ToLower().Contains("all");
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            try 
            {
                var builder = new StringBuilder();
                builder.Append("Fine Count:\n");
            
                var slackUserIds = context.Message.GetSlackIdsFromMessageExcluding(context.BotUserID);

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
            catch (Exception ex)
            {
                return this.GetExceptionResponse(ex);
            }
        }
    }
}
