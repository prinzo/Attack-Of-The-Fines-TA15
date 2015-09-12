using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FineBot.BotRunner.Extensions;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class WhatIsAFineResponder
    {
        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded && context.Message.IsWhatIsAFine();
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            
        }
    }
}
