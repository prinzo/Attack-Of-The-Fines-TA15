using System;
using System.Collections.Generic;
using System.Text;
using FineBot.API.FinesApi;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;
using MargieBot.Responders;

namespace FineBot.BotRunner.Responders
{
    public class YouTubeResponder : IFineBotResponder
    {
        private readonly IUserApi userApi;
        private readonly IFineApi fineApi;

        public YouTubeResponder(IUserApi userApi, IFineApi fineApi)
        {
            this.userApi = userApi;
            this.fineApi = fineApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded && context.Message.IsYouTubeLink();
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            var youtubeLinkList = context.Message.GetYouTubeLinkList();
            const string reasonForOneVideo = "for sharing the following YouTube video --> ";
            const string reasonForManyVideos = "for sharing the following YouTube videos --> ";
            
            var builder = new StringBuilder();
            builder.Append(context.FormattedBotUserID());
            builder.Append(": fine ");
            builder.Append(context.Message.User.FormattedUserID);
            builder.Append(" ");
            builder.Append(youtubeLinkList.Count == 1 ? reasonForOneVideo : reasonForManyVideos);

            var issuer = userApi.GetUserBySlackId(context.BotUserID);
            var recipient = userApi.GetUserBySlackId(context.Message.User.FormattedUserID);
            var seconder = recipient;

            for (var i = 0; i < youtubeLinkList.Count; i++)
            {
                fineApi.IssueAutoFine(issuer.Id, recipient.Id, seconder.Id, reasonForOneVideo + youtubeLinkList[i]);
                builder.Append(youtubeLinkList[i]);
                AddConjunctionOrSeparator(builder, youtubeLinkList, i);
            }
            
            return new BotMessage{ Text = builder.ToString() };
        }

        private static void AddConjunctionOrSeparator(StringBuilder builder, IReadOnlyCollection<string> youtubeLinkList, int currentIndex)
        {
            if (youtubeLinkList.Count == 2 && currentIndex == 0)
            {
                builder.Append(" and ");
            }
            else if (youtubeLinkList.Count > 2)
            {
                if (currentIndex < youtubeLinkList.Count - 2)
                {
                    builder.Append(", ");
                }
                else if (currentIndex == youtubeLinkList.Count - 2)
                {
                    builder.Append(", and ");
                }
            }
        }
    }
}
