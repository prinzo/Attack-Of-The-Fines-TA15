using System;
using System.Text.RegularExpressions;
using FineBot.API.SupportApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using FineBot.Enums;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class SupportResponder : ResponderBase, IFineBotResponder
    {
        public SupportResponder(ISupportApi supportApi) : base(supportApi)
        {
            
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
                var supportTypes = String.Join("|", Enum.GetNames(typeof(SupportType)));

                Regex regex = new Regex(String.Format("support ({0}) (.*)", supportTypes), RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var match = regex.Match(context.Message.Text);

                return new BotMessage();
            }
            catch(Exception ex)
            {
                return this.GetExceptionResponse(ex);
            }
        }
    }
}