using System;
using System.Linq;
using System.Text.RegularExpressions;
using FineBot.API.FinesApi;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Models;
using MargieBot.Models;
using MargieBot.Responders;
using ServiceStack.Text;

namespace FineBot.BotRunner.Responders
{
    public class PayFineResponder : IResponder
    {
        private readonly IUserApi userApi;
        private readonly IFineApi fineApi;

        public PayFineResponder(
            IUserApi userApi,
            IFineApi fineApi
            )
        {
            this.userApi = userApi;
            this.fineApi = fineApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded
                   && context.Message.MentionsBot
                   && (     context.Message.MatchesRegEx(@"pay [0-9]+ fines for") 
                         || context.Message.MatchesRegEx(@"pay [0-9]+ fine for")
                         || context.Message.MatchesRegEx(@"pay fine for")
                         );
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            var user = this.GetUser(context);

            var number = this.GetNumberOfFinesPaid(context);

            var image = this.GetImage(context.Message);

            this.fineApi.PayFines(user.Id, number, image);

            string s = Convert.ToInt32(number) > 1 ? "s" : string.Empty;

            return new BotMessage { Text = String.Format("{0} fine{1} paid for {2}!", number, s, user.SlackId) };
        }

        private byte[] GetImage(SlackMessage message)
        {
            var jsonObject = new JsonSerializer<SlackRawMessageModel>().DeserializeFromString(message.RawData);

            return new byte[1];
        }

        private int GetNumberOfFinesPaid(ResponseContext context)
        {
            var regex = new Regex(@"pay ([0-9]+) fines? for", RegexOptions.Compiled);

            var number = context.Message.GetRegexMatch(regex).Groups[1].Value;

            if(number == string.Empty)
            {
                number = "1";
            }

            return Convert.ToInt32(number);
        }

        private UserModel GetUser(ResponseContext context)
        {
            var users = context.Message.GetUserIdsFromMessageExcluding(context.BotUserID);

            var user = this.userApi.GetUserBySlackId(users.First());

            return user;
        }
    }
}