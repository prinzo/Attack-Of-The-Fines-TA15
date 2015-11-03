using System.Text;
using System.Text.RegularExpressions;

namespace FineBot.BotRunner.Extensions
{
    public static class StringExtensions
    {
        public static bool IsYouTubeLink(this string text)
        {
            var youtubeRegex = new Regex(@"(youtube.com\/watch\?|youtu.be\/)", RegexOptions.Compiled);
            return youtubeRegex.IsMatch(text);
        }

        public static string ToHyperlink(this string text, string url)
        {
            var builder = new StringBuilder();
            builder.Append("<");
            builder.Append(url);
            builder.Append("|");
            builder.Append(text);
            builder.Append(">");
            return builder.ToString();
        }

        public static string FormatAsSlackUserId(this string slackId)
        {
            return slackId.StartsWith("<@") && slackId.EndsWith(">") ? slackId : new StringBuilder().Append("<@").Append(slackId).Append(">").ToString();
        }

        public static string RemoveFormattingFromSlackUserId(this string slackId)
        {
            return slackId.StartsWith("<") ? slackId.Substring(2, slackId.Length - 3) : slackId;
        }
    }
}
