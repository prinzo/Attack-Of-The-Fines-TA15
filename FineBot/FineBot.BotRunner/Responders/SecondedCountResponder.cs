using System;
using System.Linq;
using System.Text;
using FineBot.API.ReactionApi;
using FineBot.API.SupportApi;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class SecondedCountResponder : ResponderBase, IFineBotResponder
    {
        private readonly IUserApi userApi;

        public SecondedCountResponder(IUserApi userApi, 
            ISupportApi supportApi,
            IReactionApi reactionApi)
            : base(supportApi, reactionApi)
        {
            this.userApi = userApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded
                   && context.Message.MentionsBot
                   && context.Message.Text.ToLower().Contains("seconded count");
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            try
            {
                var builder = new StringBuilder();
                builder.Append("Seconded count:\n");

                var slackUserIds = context.Message.GetSlackIdsFromMessageExcluding(context.BotUserID);
                if (!slackUserIds.Any())
                {
                    slackUserIds.Add(context.Message.User.FormattedUserID);
                }

                foreach (var slackUserId in slackUserIds)
                {
                    var user = userApi.GetUserBySlackId(slackUserId);
                    var secondedFineCount = userApi.GetFinesSecondedByUserCount(user.Id);

                    builder.Append(slackUserId);
                    builder.Append(" - ");
                    builder.Append(secondedFineCount);
                    builder.AppendLine();
                }

                return new BotMessage{ Text = builder.ToString() };
            }
            catch (Exception ex)
            {
                return this.GetExceptionResponse(ex, context.Message);
            }
        }
    }
}
