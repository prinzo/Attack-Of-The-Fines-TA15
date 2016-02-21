using FineBot.API.ReactionApi;
using FineBot.API.SupportApi;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;

namespace FineBot.BotRunner.Responders
{
    public class SecondAutofineResponder : ResponderBase, ISecondCousinResponder
    {
        private readonly IUserApi userApi;

        public SecondAutofineResponder(
            IUserApi userApi,
            ISupportApi supportApi,
            IReactionApi reactionApi
            )
            : base(supportApi, reactionApi)
        {
            this.userApi = userApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded
                   && context.IsMessageFromUser("finesbot")
                   && context.Message.Text.ToLower().Contains("auto-fine");
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            return new BotMessage{ Text = "Seconded!"};
        }
    }
}
