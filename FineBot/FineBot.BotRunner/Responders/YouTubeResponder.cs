using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using MargieBot.Models;
using MargieBot.Responders;

namespace FineBot.BotRunner.Responders
{
    public class YouTubeResponder : IResponder
    {
        private readonly IUserApi userApi;

        public YouTubeResponder(IUserApi userApi)
        {
            this.userApi = userApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return context.Message.IsYouTubeLink();
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            var builder = new StringBuilder();
            builder.Append("<@");
            builder.Append(context.BotUserID);
            builder.Append(">: fine ");
            builder.Append(context.Message.User.FormattedUserID);
            return new BotMessage{ Text = builder.ToString() };
        }
    }
}
