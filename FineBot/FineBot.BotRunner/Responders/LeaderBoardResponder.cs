using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FineBot.API.UsersApi;
using MargieBot.Models;
using MargieBot.Responders;

namespace FineBot.BotRunner.Responders
{
    public class LeaderBoardResponder : IResponder
    {
        private readonly IUserApi userApi;

        public LeaderBoardResponder(IUserApi userApi)
        {
            this.userApi = userApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return context.Message.MentionsBot && context.Message.Text.Contains("leaderboard");
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            //today, this week, this month, this year, all time
            var builder = new StringBuilder();
            var leaderboard = new List<UserModel>();
            const int sizeOfLeaderBoard = 10;

            var text = context.Message.Text.ToLower();
            if (text.Contains("today"))
            {
                builder.AppendLine("Leaderboard for today");
                leaderboard = this.userApi.GetLeaderboardToday(sizeOfLeaderBoard);
            }
            else if (text.Contains("this week"))
            {
                builder.AppendLine("Leaderboard for this week");
                leaderboard = this.userApi.GetLeaderboardForThisWeek(sizeOfLeaderBoard);
            }
            else if (text.Contains("this month"))
            {
                builder.AppendLine("Leaderboard for this month");
                leaderboard = this.userApi.GetLeaderboardForThisMonth(sizeOfLeaderBoard);
            }
            else if (text.Contains("this year"))
            {
                builder.AppendLine("Fines Leaderboard for this year");
                leaderboard = this.userApi.GetLeaderboardForThisYear(sizeOfLeaderBoard);
            }
            else
            {
                builder.AppendLine("Fines Leaderboard All Time:");
                leaderboard = this.userApi.GetLeaderboard(sizeOfLeaderBoard);
            }

            foreach(var userModel in leaderboard)
            {
                builder.AppendLine(String.Format("{0} - {1}", userModel.SlackId, userModel.AwardedFineCount));
            }

            return new BotMessage{Text = builder.ToString()};
        }
    }
}