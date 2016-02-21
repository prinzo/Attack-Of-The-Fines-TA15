using System;
using System.Text.RegularExpressions;
using FineBot.API.ReactionApi;
using FineBot.API.SupportApi;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using FineBot.Enums;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class SupportResponder : ResponderBase, IFineBotResponder
    {
        private readonly IUserApi userApi;

        public SupportResponder(
            ISupportApi supportApi,
            IUserApi userApi,
            IReactionApi reactionApi
            ) : base(supportApi, reactionApi)
        {
            this.userApi = userApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return context.Message.MentionsBot
                   && !context.BotHasResponded
                   && context.Message.MatchesRegEx(@"support");

        }

        public BotMessage GetResponse(ResponseContext context)
        {
            try
            {
                var user = this.userApi.GetUserBySlackId(context.Message.User.FormattedUserID);

                var supportTypes = String.Join("|", Enum.GetNames(typeof(SupportType)));

                Regex regex = new Regex(String.Format("support ({0}) (.*)", supportTypes), RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var match = regex.Match(context.Message.Text);

                if(match.Groups.Count > 2)
                {
                    this.LogSupportTicketWithType(match, user);
                }
                else
                {
                    var message = context.Message.Text.Replace("support", "");

                    this.LogSupportTicket(user, message, SupportType.General);
                }

                return new BotMessage()
                       {
                           Text = "A support ticket has been created for your issue."
                       }; 
            }
            catch(Exception ex)
            {
                return this.GetExceptionResponse(ex, context.Message);
            }
        }

        private void LogSupportTicketWithType(Match match, UserModel user)
        {
            SupportType supportType;

            var couldParseEnum = Enum.TryParse(match.Groups[1].Value, true, out supportType);

            if(!couldParseEnum)
            {
                supportType = SupportType.General;
            }

            var message = match.Groups[2].Value;

            this.LogSupportTicket(user, message, supportType);
        }

        private void LogSupportTicket(UserModel user, string message, SupportType supportType)
        {
            this.supportApi.CreateSupportTicketOnTrello(new SupportTicketModel()
                                                        {
                                                            Message = String.Format("Support Ticket Logged From Bot: {0}", message),
                                                            Subject = String.Format("Support Ticket from {0}", user.DisplayName),
                                                            Status = 1,
                                                            Type = (int)supportType
                                                        });
        }
    }
}