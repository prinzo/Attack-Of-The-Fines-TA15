using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class FineCountResponder : IFineBotResponder
    {
        public bool CanRespond(ResponseContext context)
        {
            return false;
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            return new BotMessage{ Text = "in progress" };
        }
    }
}
