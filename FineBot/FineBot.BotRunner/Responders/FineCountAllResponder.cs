using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FineBot.API.FinesApi;
using FineBot.API.SupportApi;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class FineCountAllResponder : ResponderBase, IFineBotResponder
    {
        private readonly IFineApi fineApi;

        public FineCountAllResponder(IFineApi fineApi, ISupportApi supportApi) : base(supportApi)
        {
            this.fineApi = fineApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded 
                && context.Message.MentionsBot 
                && context.Message.Text.ToLower().Contains("fine count all");
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            try
            {
                var builder = new StringBuilder();
                builder.Append("Total fines issued by all users: ");
                builder.Append(fineApi.CountAllFinesSuccessfullyIssued());
                
                return new BotMessage{ Text = builder.ToString() };
            }
            catch(Exception ex)
            {
                return this.GetExceptionResponse(ex);
            }
        }
    }
}
