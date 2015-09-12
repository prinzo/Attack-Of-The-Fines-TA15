using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MargieBot.Models;

namespace FineBot.BotRunner.Extensions
{
    public static class SlackMessageExtensions
    {
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

        public static bool IsWhatIsAFine(this SlackMessage message)
        {
            var lowercaseText = message.Text.ToLower();
            return (lowercaseText.Contains("what is a fine") || lowercaseText.Contains("whats a fine") ||
                    lowercaseText.Contains("what's a fine"));
        }
    }
}