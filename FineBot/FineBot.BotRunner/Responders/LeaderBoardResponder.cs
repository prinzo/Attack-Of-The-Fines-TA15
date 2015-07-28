using System;
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
            var leaderboard = this.userApi.GetLeaderboard(10);

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("Fines Leaderboard:");

            foreach(var userModel in leaderboard)
            {
                builder.AppendLine(String.Format("{0} - {1}", userModel.SlackId, userModel.AwardedFineCount));
            }

            return new BotMessage{Text = builder.ToString()};
        }
    }
}