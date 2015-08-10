using System;
using System.Text;
using FineBot.API.FinesApi;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using MargieBot.Models;
using MargieBot.Responders;

namespace FineBot.BotRunner.Responders
{
    public class YouTubeResponder : IResponder
    {
        private readonly IUserApi userApi;
        private readonly IFineApi fineApi;

        public YouTubeResponder(IUserApi userApi, IFineApi fineApi)
        {
            this.userApi = userApi;
            this.fineApi = fineApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded && context.Message.IsYouTubeLink();
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            var issuer = this.userApi.GetUserBySlackId(context.BotUserID);
            if (issuer == null)
            {
                return new BotMessage{Text = String.Format("Sorry {0}, you may not issue fines until you have registered, say 'Register me' to me and you'll be in the system!", 
                    "<@" + context.BotUserID + ">")};
            }

            const string reason = "for a YouTube link";

            this.FineRecipient(context.Message.User.FormattedUserID, issuer, reason);

            var seconder = this.userApi.GetUserBySlackId(context.Message.User.FormattedUserID);

            FineWithUserModel secondedFine = this.fineApi.SecondNewestPendingFine(seconder.Id);

            if (secondedFine == null)
            {
                return new BotMessage { Text = String.Format("Sorry {0}, there are no pending fines to second", context.Message.User.FormattedUserID) };
            }

            var builder = new StringBuilder();
            builder.Append("<@");
            builder.Append(context.BotUserID);
            builder.Append(">: fine ");
            builder.Append(context.Message.User.FormattedUserID);
            builder.Append(" ");
            builder.Append(reason);

            return new BotMessage{ Text = builder.ToString() };
        }

        private void FineRecipient(string userId, UserModel issuer, string reason)
        {
            var userModel = this.userApi.GetUserBySlackId(userId);
            this.fineApi.IssueFine(issuer.Id, userModel.Id, reason);
        }
    }
}
