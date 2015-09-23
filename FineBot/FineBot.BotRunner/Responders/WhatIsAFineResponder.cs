using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class WhatIsAFineResponder : IFineBotResponder
    {
        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded && context.Message.IsWhatIsAFine();
        }

        public BotMessage GetResponse(ResponseContext context)
        {
             return new BotMessage{Text = "in progress"};
        }
    }
}
