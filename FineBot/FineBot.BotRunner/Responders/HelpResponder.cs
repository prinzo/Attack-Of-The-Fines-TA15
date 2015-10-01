using System.Text;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;
using MargieBot.Responders;

namespace FineBot.BotRunner.Responders
{
    public class HelpResponder : IFineBotResponder
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
            builder.AppendLine("@FinesBot pay <number of fines> for @<slack user>: Pay the specified number of fines for the specified user. Note: You must include an image of the fine payment and may not pay your own fines!");
            builder.AppendLine("@FinesBot leaderboard: Show a leaderboard of the top ten users with the most fines.");
            builder.AppendLine("@FinesBot leaderboard today: Show a leaderboard of the top ten users with the most fines for the current day.");
            builder.AppendLine("@FinesBot leaderboard this week: Show a leaderboard of the top ten users with the most fines for the current week starting Monday.");
            builder.AppendLine("@FinesBot leaderboard this month: Show a leaderboard of the top ten users with the most fines for the current month.");
            builder.AppendLine("@FinesBot leaderboard this year: Show a leaderboard of the top ten users with the most fines for the current year.");

            return new BotMessage{Text = builder.ToString()};
        }
    }
}