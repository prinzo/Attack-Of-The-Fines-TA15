using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FineBot.API.UsersApi;
using MargieBot.Models;
using MargieBot.Responders;

namespace FineBot.BotRunner.Responders
{
    public class LeaderBoardTodayResponder : IResponder
    {
        private readonly IUserApi userApi;

        public LeaderBoardTodayResponder(IUserApi userApi)
        {
            this.userApi = userApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return context.Message.MentionsBot && context.Message.Text.Contains("today");
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            var leaderboard = this.userApi.GetLeaderboardToday(10);

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("Fines Leaderboard For Today(" + DateTime.Today.ToShortDateString() + "):");

            foreach (var userModel in leaderboard)
            {
                builder.AppendLine(String.Format("{0} - {1}", userModel.SlackId, userModel.FineCount));
            }

            return new BotMessage { Text = builder.ToString() };
        }
    }
}
