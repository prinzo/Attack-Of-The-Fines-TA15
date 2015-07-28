using System.Text;
using MargieBot.Models;
using MargieBot.Responders;

namespace FineBot.BotRunner.Responders
{
    public class HelpResponder : IResponder
    {
        public bool CanRespond(ResponseContext context)
        {
            return context.Message.MentionsBot
                && !context.BotHasResponded
                && context.Message.Text.ToLower().Contains("help");
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("I am the Fines Bot! Below you'll find the commands I can accept and a brief description:");
            builder.AppendLine("----------------------------------------------------------------------------------------");

            builder.AppendLine("@FinesBot fine @<slack user>: Award a fine to a slack user, you can fine multiple people at once.");
            builder.AppendLine("seconded!: Second the oldest awarded fine that was not seconded.");
            builder.AppendLine("@FinesBot leaderboard: Show a leaderboard of the top ten users with the most fines.");

            return new BotMessage{Text = builder.ToString()};
        }
    }
}