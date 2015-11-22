using System.Linq;
using System.Text;
using MargieBot.Models;

namespace FineBot.BotRunner.Extensions
{
    public static class ResponseContextExtensions
    {
        public static string FormattedBotUserID(this ResponseContext context)
        {
            var builder = new StringBuilder();
            builder.Append("<@");
            builder.Append(context.BotUserID);
            builder.Append(">");
            
            return builder.ToString();
        }

        public static bool IsMessageFromUser(this ResponseContext context, string username)
        {
            return context.UserNameCache[context.Message.User.ID].Equals(username);
        }

        public static string GetCommandMentioningBot(this ResponseContext context, string command)
        {
            return string.Format("{0}: {1}", context.FormattedBotUserID(), command);
        }

        public static string GetSecondCousinSlackId(this ResponseContext context)
        {
            foreach (var slackIdUsernamePair in context.UserNameCache.Where(x => x.Value.Equals("finebotssecondcousin")))
            {
                return slackIdUsernamePair.Key.FormatAsSlackUserId();
            }
            return "";
        }
    }
}
