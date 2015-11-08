using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Castle.Core.Internal;
using FineBot.API.FinesApi;
using FineBot.API.SupportApi;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Models;
using FineBot.BotRunner.Responders.Interfaces;
using FineBot.Common.Enums;
using FineBot.Common.Infrastructure;
using MargieBot.Models;
using MargieBot.Responders;
using ServiceStack.Text;

namespace FineBot.BotRunner.Responders
{
    public class PayFineResponder : ResponderBase, IFineBotResponder
    {
        private readonly IUserApi userApi;
        private readonly IFineApi fineApi;

        public PayFineResponder(
            IUserApi userApi,
            IFineApi fineApi,
            ISupportApi supportApi
            ) : base(supportApi)
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
                         || context.Message.MatchesRegEx(@"pay [0-9]+ for")
                         || context.Message.MatchesRegEx(@"pay fine for")
                         );
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            try
            {
                var user = this.GetUser(context);

                var payer = this.userApi.GetUserBySlackId(context.Message.User.FormattedUserID);

                var number = this.GetNumberOfFinesPaid(context);

                PaymentImageModel paymentImage = this.GetImage(context.Message);

                if(paymentImage == null)
                {
                    return this.GetErrorResponse(new ValidationResult().AddMessage(Severity.Error, "You must upload an image to pay fines!"));
                }

                ValidationResult result = this.fineApi.PayFines(user.Id, payer.Id, number, paymentImage);

                if(result.HasErrors)
                {
                    return this.GetErrorResponse(result);
                }

                string s = Convert.ToInt32(number) > 1 ? "s" : string.Empty;

                return new BotMessage { Text = String.Format("{0} fine{1} paid for {2}!", number, s, user.SlackId) };
            }
            catch(Exception ex)
            {
                return this.GetExceptionResponse(ex);
            }
        }

        private PaymentImageModel GetImage(SlackMessage message)
        {
            var rawMessageModel = new JsonSerializer<SlackRawMessageModel>().DeserializeFromString(message.RawData);

            if(rawMessageModel.file == null || string.IsNullOrEmpty(rawMessageModel.file.url))
            {
                return null;
            }

            byte[] data;

            using (WebClient client = new WebClient())
            {
                data = client.DownloadData(rawMessageModel.file.url);
            }

            return new PaymentImageModel
                   {
                       FileName = rawMessageModel.file.name,
                       ImageBytes = data,
                       MimeType = rawMessageModel.file.mimetype
                   };
        }

        private int GetNumberOfFinesPaid(ResponseContext context)
        {
            var regex = new Regex(@"pay ([0-9]+) fines? for", RegexOptions.Compiled);

            var number = context.Message.GetRegexMatch(regex).Groups[1].Value;

            if(number != string.Empty)
            {
                return Convert.ToInt32(number);
            }

            regex = new Regex(@"pay ([0-9]+) for", RegexOptions.Compiled);
                
            number = context.Message.GetRegexMatch(regex).Groups[1].Value;

            if(number == string.Empty)
            {
                number = "1";                    
            }

            return Convert.ToInt32(number);
        }

        private UserModel GetUser(ResponseContext context)
        {
            var users = context.Message.GetSlackIdsFromMessageExcluding(context.BotUserID);

            if(users.IsNullOrEmpty())
            {
                
            }

            var user = this.userApi.GetUserBySlackId(users.First());

            return user;
        }
    }
}