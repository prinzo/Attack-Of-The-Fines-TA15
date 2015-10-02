using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using MargieBot.Models;

namespace FineBot.BotRunner.Extensions
{
    public static class SlackMessageExtensions
    {
        private static readonly List<string> CommonReplyList; 

        static SlackMessageExtensions()
        {
            CommonReplyList = new List<string>
            {
                "it's never done that before",
                "it worked yesterday",
                "how is that possible",
                "it must be a hardware problem",
                "what did you type wrong to get it to crash",
                "there has to be something wrong in your data",
                "i haven't touched that project in weeks",
                "you must have the wrong version",
                "i can't test everything",
                "it works, but hasn't been tested",
                "somebody must have changed my code",
                "even though it doesn't work, how does it feel",
                "you can't use that version on your system",
                "why do you want to do it that way",
                "where were you when the program blew up",
                "what is a fine",
                "whats a fine",
                "what's a fine",
                "it works on my machine"
            };
        }

        public static bool MatchesRegEx(this SlackMessage message, string expression)
        {
            return Regex.IsMatch(message.Text, expression);
        }

        public static Match GetRegexMatch(this SlackMessage message, Regex expression)
        {
            return expression.Match(message.Text);
        }

        public static List<string> GetUserIdsFromMessageExcluding(this SlackMessage message, string usernameToExclude)
        {
            Regex usernameRegex = new Regex(@"<@(?<username>\w+?)>", RegexOptions.Compiled);

            List<string> usernames = (from Match matchedUsername in usernameRegex.Matches(message.Text) select matchedUsername.Value).ToList();

            return usernames.Where(username => !username.Contains(usernameToExclude)).ToList();
        }

        public static bool IsYouTubeLink(this SlackMessage message)
        {
            return message.Text.IsYouTubeLink();
        }

        public static List<string> GetYouTubeLinkList(this SlackMessage message)
        {
            var youtubeLinkList = new List<string>();
            if (!message.IsYouTubeLink()) return youtubeLinkList;

            var urlRegex = new Regex(@"<(http|https):\/\/.[^<>]*>", RegexOptions.Compiled);
            var matches = urlRegex.Matches(message.Text);
            foreach (Match match in matches)
            {
                if(match.Value.IsYouTubeLink())
                    youtubeLinkList.Add(match.Value);
            }
            return youtubeLinkList;
        }

        public static bool IsCommonReply(this SlackMessage message)
        {
            var lowercaseText = message.Text.ToLower();
            return CommonReplyList.Any(x => lowercaseText.Contains(x));
        }
    }
}