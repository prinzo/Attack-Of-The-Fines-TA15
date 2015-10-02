using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class SecondAutofineResponder : ISecondCousinResponder
    {
        private readonly IUserApi userApi;

        public SecondAutofineResponder(IUserApi userApi)
        {
            this.userApi = userApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded
                   && context.UserNameCache[context.Message.User.ID].Equals("finesbot")
                   && context.Message.Text.ToLower().Contains("fine");
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            return new BotMessage{ Text = "Seconded!"};
        }
    }
}
