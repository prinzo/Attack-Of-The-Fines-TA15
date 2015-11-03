using System;
using System.Linq;
using System.Text;
using FineBot.API.SupportApi;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class ShowResponder : ResponderBase, IFineBotResponder
    {
        private readonly IUserApi userApi;

        public ShowResponder(
            IUserApi userApi,
            ISupportApi supportApi
            )
            : base(supportApi)
        {
            this.userApi = userApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return context.Message.MentionsBot 
                &&!context.BotHasResponded
                && context.Message.StartsWithCommand("show");
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            try
            {

                if (context.Message.Text.ToLower().Contains("pending"))
                {
                    var fines = from user in this.userApi.GetUsersWithPendingFines()
                                from fine in user.Fines
                                where fine.Pending
                                select new { user, fine };

                    StringBuilder builder = new StringBuilder();

                    builder.AppendLine("Pending Fines:");
                    builder.AppendLine("--------------------");

                    foreach (var userModel in fines)
                    {
                        builder.AppendLine(String.Format("{0} {1}", userModel.user.SlackId.FormatAsSlackUserId(), userModel.fine.Reason));
                    }

                    return new BotMessage { Text = builder.ToString() };
                }

                return new BotMessage { Text = "Show what?" };
            }
            catch (Exception ex)
            {
                return this.GetExceptionResponse(ex);
            }
        }
    }
}