﻿using System.Configuration;
using Flurl.Http;
using ServiceStack.Text;

namespace FineBot.API.ReactionApi
{
    public class ReactionApi : IReactionApi
    {
        public bool AddReaction(string reaction, string channel, string timestamp)
        {
            var responseString = "https://slack.com/api/reactions.add"
                .PostUrlEncodedAsync(
                    new
                    {
                        token = ConfigurationManager.AppSettings["SlackApiKey"],
                        name = reaction,
                        channel = channel,
                        timestamp = timestamp
                    })
                .ReceiveString()
                .Result;

            var serializer = new JsonSerializer<ReactionResponseModel>();
            var response = serializer.DeserializeFromString(responseString);
            return response.ok;
        }
    }
}
