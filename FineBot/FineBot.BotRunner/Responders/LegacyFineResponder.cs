using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FineBot.API.FinesApi;
using FineBot.API.SupportApi;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;
using MargieBot.Responders;

namespace FineBot.BotRunner.Responders
{
    public class LegacyFineResponder : ResponderBase, IFineBotResponder
    {
        private readonly IUserApi userApi;
        private readonly IFineApi fineApi;

        public LegacyFineResponder(IUserApi userApi, IFineApi fineApi, ISupportApi supportApi) : base(supportApi)
        {
            this.userApi = userApi;
            this.fineApi = fineApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded && context.Message.Text.ToLower().Contains("legacy");
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            try
            {
                var slackId = context.Message.GetSlackIdsFromMessageExcluding(context.BotUserID).FirstOrDefault();

                if (slackId == null) return new BotMessage { Text = "Enter a person to fine" };

                var issuer = this.GetIssuer(context);

                var recipient = userApi.GetUserBySlackId(slackId);

                var seconderSlackId = GetSlackIdOfCousin(context);
                if (seconderSlackId.Equals("error")) return new BotMessage { Text = "cousin not in channel or wrong name" };

                var seconder = userApi.GetUserBySlackId(seconderSlackId);

                const string reason = "for fines on board";

                var numberOfFines = GetNumberOfFines(context.Message.Text);

                for(var i = 0; i < numberOfFines; i++) 
                {
                    IssueFineResult result = this.fineApi.IssueFine(issuer.Id, recipient.Id, reason);

                    if (result.HasErrors)
                    {
                        return this.GetErrorResponse(result);
                    }

                    fineApi.SecondNewestPendingFine(seconder.Id);
                }

                return new BotMessage { Text = "Issued." };
            }
            catch (Exception ex)
            {
                return this.GetExceptionResponse(ex);
            }
        }

        private int GetNumberOfFines(string text)
        {
            var words = text.Split(' ');
            foreach (var word in words)
            {
                var number = 0;
                var result = Int32.TryParse(word, out number);
                if (result) return number;
            }
            return 1;
        }

        private UserModel GetIssuer(ResponseContext context)
        {
            var issuer = this.userApi.GetUserBySlackId(context.Message.User.FormattedUserID);

            return issuer;
        }

        private string GetSlackIdOfCousin(ResponseContext context)
        {
            foreach (var usernameKeyValue in context.UserNameCache.Where(usernameKeyValue => usernameKeyValue.Value.Equals("finebotssecondcousin")))
            {
                return usernameKeyValue.Key;
            }
            return "error";
        }
    }
}
