using System;
using System.Text;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using FineBot.Enums;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class HelpResponder : IFineBotResponder
    {
        public bool CanRespond(ResponseContext context)
        {
            var command = context.GetCommandMentioningBot("help");

            return context.Message.MentionsBot
                && !context.BotHasResponded
                && context.Message.Text.ToLower().Contains(command.ToLower());
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("I am the Fines Bot! Below you'll find the commands I can accept and a brief description:");
            builder.AppendLine("----------------------------------------------------------------------------------------");

            builder.AppendLine("@FinesBot fine @<slack user>: Award a fine to a slack user, you can fine multiple people at once.");
            builder.AppendLine();
            builder.AppendLine("seconded!: Second the oldest awarded fine that was not seconded.");
            builder.AppendLine();
            builder.AppendLine("@FinesBot pay <number of fines> fine(s) for @<slack user>: Pay the specified number of fines for the specified user. Note: You must include an image of the fine payment and may not pay your own fines!");
            builder.AppendLine();
            builder.AppendLine("@FinesBot leaderboard: Show a leaderboard of the top ten users with the most fines.");
            builder.AppendLine();
            builder.AppendLine("@FinesBot leaderboard today: Show a leaderboard of the top ten users with the most fines for the current day.");
            builder.AppendLine();
            builder.AppendLine("@FinesBot leaderboard this week: Show a leaderboard of the top ten users with the most fines for the current week starting Monday.");
            builder.AppendLine();
            builder.AppendLine("@FinesBot leaderboard this month: Show a leaderboard of the top ten users with the most fines for the current month.");
            builder.AppendLine();
            builder.AppendLine("@FinesBot leaderboard this year: Show a leaderboard of the top ten users with the most fines for the current year.");
            builder.AppendLine();
            builder.AppendLine(String.Format("@FinesBot support <support type: {0}> <message>: Creates a support ticket with the status selected. If no status is specified it is created as a general issue.", String.Join("|", Enum.GetNames(typeof(SupportType)))));

            return new BotMessage{Text = builder.ToString()};
        }
    }
}