using System;
using System.Linq;
using System.Text;
using FineBot.API.UsersApi;
using MargieBot.Models;
using MargieBot.Responders;

namespace FineBot.BotRunner.Responders
{
    public class ShowResponder : IResponder
    {
        private readonly IUserApi userApi;

        public ShowResponder(
            IUserApi userApi
            )
        {
            this.userApi = userApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return context.Message.MentionsBot 
                &&!context.BotHasResponded
                && context.Message.Text.ToLower().Contains("show");
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            if(context.Message.Text.ToLower().Contains("pending"))
            {
                var fines = from user in this.userApi.GetUsersWithPendingFines()
                    from fine in user.Fines
                    where fine.Pending
                    select new { user, fine };

                StringBuilder builder = new StringBuilder();

                builder.AppendLine("Pending Fines:");
                builder.AppendLine("--------------------");

                foreach(var userModel in fines)
                {
                    builder.AppendLine(String.Format("{0} {1}", userModel.user.SlackId, userModel.fine.Reason ));
                }

                return new BotMessage{Text = builder.ToString()};
            }

            return new BotMessage{ Text = "Show what?"};
        }
    }
}