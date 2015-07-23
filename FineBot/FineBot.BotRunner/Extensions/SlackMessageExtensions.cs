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
    }
}