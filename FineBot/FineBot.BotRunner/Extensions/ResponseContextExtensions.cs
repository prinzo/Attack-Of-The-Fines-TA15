using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
