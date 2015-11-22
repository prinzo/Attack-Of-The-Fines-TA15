﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using FineBot.API.FinesApi;
using FineBot.API.ReactionApi;
using FineBot.API.SupportApi;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Extensions;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;
using ServiceStack;

namespace FineBot.BotRunner.Responders
{
    public class CommonReplyResponder : ResponderBase, IFineBotResponder
    {
        private readonly IUserApi userApi;
        private readonly IFineApi fineApi;
        private readonly IReactionApi reactionApi;

        public CommonReplyResponder(
            IUserApi userApi, 
            IFineApi fineApi,
            ISupportApi supportApi,
            IReactionApi reactionApi
            ) : base (supportApi)
        {
            this.userApi = userApi;
            this.fineApi = fineApi;
            this.reactionApi = reactionApi;
        }

        public bool CanRespond(ResponseContext context)
        {
            return !context.BotHasResponded && context.Message.IsCommonReply();
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            try
            {
                const string reason = "for a common reply";

                var builder = new StringBuilder();
                builder.Append(context.FormattedBotUserID());
                builder.Append(": ");
                builder.Append("auto-fine".ToHyperlink(ConfigurationManager.AppSettings["ShameBellLocation"]));
                builder.Append(" ");
                builder.Append(context.Message.User.FormattedUserID);
                builder.Append(" ");
                builder.Append(reason);
                if(context.Message.Text.Contains("it works on my machine"))
                {
                    reactionApi.AddReaction(
                        "itworksonmymachine", 
                        context.Message.GetChannelId(),
                        context.Message.GetTimeStamp());
                }
                var issuer = userApi.GetUserBySlackId(context.FormattedBotUserID());
                var recipient = userApi.GetUserBySlackId(context.Message.User.FormattedUserID);
                var seconderSlackId = context.GetSecondCousinSlackId();
                if (seconderSlackId.Equals("")) seconderSlackId = recipient.SlackId;
                var seconder = userApi.GetUserBySlackId(seconderSlackId);

                fineApi.IssueAutoFine(issuer.Id, recipient.Id, seconder.Id, reason);

                return new BotMessage { Text = builder.ToString()};
            }
            catch(Exception ex)
            {
                return this.GetExceptionResponse(ex);
            }
        }
    }
}
