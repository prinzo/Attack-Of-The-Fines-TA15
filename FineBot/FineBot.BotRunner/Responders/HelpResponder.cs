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
            var builder = new StringBuilder();

            builder.AppendLine("@finesbot fine @<slack user> for <reason>: Award a fine to a slack user, you can fine multiple people at once.");
            builder.AppendLine();
            builder.AppendLine("seconded!: Second the oldest awarded fine that was not seconded.");
            builder.AppendLine();
            builder.AppendLine("@finesbot pay <number of fines> fine(s) for @<slack user>: Pay the specified number of fines for the specified user. Note: You must include an image of the fine payment and may not pay your own fines!");
            builder.AppendLine();
            builder.AppendLine("@finesbot leaderboard <today|week|month|year>: Leaderboard of the top five users with the most fines for the current day/week/month/year. If no time period is specified, the all time leaderboard is displayed");
            builder.AppendLine();
            builder.AppendLine("@finesbot fine count <all|users>: Counts the number of fines successfully issued (i.e. seconded) by the specified users. If \"all\" is specified the count for all fines will be displayed. If no users are specified, the fine count of the requester will be displayed.");
            builder.AppendLine();
            builder.AppendLine("@finesbot seconded count <users>: Counts the number of fines seconded by the specified users. If no user is specified, the number of fines seconded by the requester will be displayed.");
            builder.AppendLine();
            builder.AppendLine(String.Format("@FinesBot support <support type: {0}> <message>: Creates a support ticket with the status selected. If no status is specified it is created as a general issue.", String.Join("|", Enum.GetNames(typeof(SupportType)))));

            return new BotMessage{Text = builder.ToString()};
        }
    }
}