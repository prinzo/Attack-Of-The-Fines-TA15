using System;
using System.Configuration;
using FineBot.API.FinesApi;
using FineBot.API.ReactionApi;
using FineBot.API.SupportApi;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class SeconderResponder : ResponderBase, IFineBotResponder
    {
        private readonly IFineApi fineApi;
        private readonly IUserApi userApi;

        public SeconderResponder(
            IFineApi fineApi,
            IUserApi userApi,
            ISupportApi supportApi, 
            IReactionApi reactionApi) : base (supportApi, reactionApi)
        {
            this.fineApi = fineApi;
            this.userApi = userApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded
                   && context.Message.Text.ToLower().StartsWith("seconded");
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            try
            {
                var slackId = context.Message.User.FormattedUserID;

                var seconder = this.userApi.GetUserBySlackId(slackId);

                var result = this.fineApi.SecondOldestPendingFine(seconder.Id);

                if (result.HasErrors || result.FineWithUserModel == null)
                {
                    return this.GetErrorResponse(result, context.Message);
                }

                var secondedFine = result.FineWithUserModel;

                var finedUser = this.GetUserName(secondedFine);

                reactionApi.AddReaction(ConfigurationManager.AppSettings["BotKey"], "ok_hand", context.Message.GetChannelId(), context.Message.GetTimeStamp());

                return new BotMessage{ Text = "" };
            }
            catch (Exception ex)
            {
                return this.GetExceptionResponse(ex, context.Message);
            }
        }

        private string GetUserName(FineWithUserModel secondedFine)
        {
            string finedUser = String.IsNullOrEmpty(secondedFine.User.SlackId)
                ? secondedFine.User.DisplayName
                : secondedFine.User.SlackId.FormatAsSlackUserId();
            return finedUser;
        }
    }
}