using System;
using FineBot.API.FinesApi;
using FineBot.API.SupportApi;
using FineBot.API.UsersApi;
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
            ISupportApi supportApi
            ) : base (supportApi)
        {
            this.fineApi = fineApi;
            this.userApi = userApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded
                && !context.UserNameCache[context.Message.User.ID].Equals("finebotssecondcousin")
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
                    return this.GetErrorResponse(result);
                }

                var secondedFine = result.FineWithUserModel;

                return new BotMessage { Text = String.Format("{0} seconded the fine given to {1} {2}!", context.Message.User.FormattedUserID, secondedFine.User.SlackId, secondedFine.Reason) };
            }
            catch (Exception ex)
            {
                return this.GetExceptionResponse(ex);
            }
        }
    }
}