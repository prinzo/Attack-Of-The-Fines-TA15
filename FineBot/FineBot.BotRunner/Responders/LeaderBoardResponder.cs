using System;
using System.Collections.Generic;
using System.Text;
using FineBot.API.ReactionApi;
using FineBot.API.SupportApi;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class LeaderBoardResponder : ResponderBase, IFineBotResponder
    {
        private readonly IUserApi userApi;

        public LeaderBoardResponder(
            IUserApi userApi,
            ISupportApi supportApi,
            IReactionApi reactionApi
            )
            : base(supportApi, reactionApi)
        {
            this.userApi = userApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return context.Message.MentionsBot
                && !context.BotHasResponded
                && context.Message.Text.Contains("leaderboard");
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            try
            {
                //today, this week, this month, this year, all time
                var builder = new StringBuilder();
                List<UserModel> leaderboard;
                const int sizeOfLeaderBoard = 5;

                var text = context.Message.Text.ToLower();
                if(text.Contains("today"))
                {
                    builder.AppendLine("Leaderboard for today");
                    leaderboard = userApi.GetLeaderboardToday(sizeOfLeaderBoard);
                }
                else if(text.Contains("week"))
                {
                    builder.AppendLine("Leaderboard for this week");
                    leaderboard = userApi.GetLeaderboardForThisWeek(sizeOfLeaderBoard);
                }
                else if(text.Contains("month"))
                {
                    builder.AppendLine("Leaderboard for this month");
                    leaderboard = userApi.GetLeaderboardForThisMonth(sizeOfLeaderBoard);
                }
                else if(text.Contains("year"))
                {
                    builder.AppendLine("Fines Leaderboard for this year");
                    leaderboard = userApi.GetLeaderboardForThisYear(sizeOfLeaderBoard);
                }
                else
                {
                    builder.AppendLine("Fines Leaderboard All Time:");
                    leaderboard = userApi.GetLeaderboard(sizeOfLeaderBoard);
                }

                foreach(var userModel in leaderboard)
                {
                    builder.AppendLine(String.Format("{0} - {1}", userModel.SlackId.FormatAsSlackUserId(), userModel.AwardedFineCount));
                }

                return new BotMessage { Text = builder.ToString() };
            }
            catch (Exception ex)
            {
                return this.GetExceptionResponse(ex, context.Message);
            }
        }
    }
}