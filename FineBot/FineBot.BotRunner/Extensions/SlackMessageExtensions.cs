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

        public static List<string> GetUserIdsFromMessageExcluding(this SlackMessage message, string usernameToExclude)
        {
            Regex usernameRegex = new Regex(@"<@(?<username>\w+?)>", RegexOptions.Compiled);

            List<string> usernames = (from Match matchedUsername in usernameRegex.Matches(message.Text) select matchedUsername.Value).ToList();

            return usernames.Where(username => !username.Contains(usernameToExclude)).ToList();
        }

        public static bool IsYouTubeLink(this SlackMessage message)
        {
            var youtubeRegex = new Regex(@"(youtube.com\/watch\?|youtu.be\/)", RegexOptions.Compiled);
            return youtubeRegex.IsMatch(message.Text);
        }
    }
}