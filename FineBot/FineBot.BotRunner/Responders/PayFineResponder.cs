using System;
using System.Linq;
using System.Text.RegularExpressions;
using FineBot.BotRunner.Extensions;
using MargieBot.Models;
using MargieBot.Responders;

namespace FineBot.BotRunner.Responders
{
    public class PayFineResponder : IResponder
    {
        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded
                   && context.Message.MentionsBot
                   && (     context.Message.MatchesRegEx(@"pay [0-9]+ fines for") 
                         || context.Message.MatchesRegEx(@"pay [0-9]+ fine for")
                         || context.Message.MatchesRegEx(@"pay fine for")
                         );
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            var regex = new Regex(@"pay ([0-9]+) fines? for", RegexOptions.Compiled);

            var users = context.Message.GetUserIdsFromMessageExcluding(context.BotUserID);

            var number = context.Message.GetRegexMatch(regex).Groups[1].Value;

            if(number == string.Empty)
            {
                number = "1";
            }

            string s = Convert.ToInt32(number) > 1 ? "s" : string.Empty;

            return new BotMessage{ Text = String.Format("{0} fine{1} paid for {2}!", number, s, users.First()) };
        }
    }
}