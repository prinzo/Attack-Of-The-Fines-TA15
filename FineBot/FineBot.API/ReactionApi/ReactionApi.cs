using FineBot.Errors;
using Flurl.Http;
using ServiceStack.Text;

namespace FineBot.API.ReactionApi
{
    public class ReactionApi : IReactionApi
    {
        public bool AddReaction(string slackApiToken, string reaction, string channel, string timestamp)
        {
            var responseString = "https://slack.com/api/reactions.add"
                .PostUrlEncodedAsync(
                    new
                    {
                        token = slackApiToken,
                        name = reaction,
                        channel = channel,
                        timestamp = timestamp
                    })
                .ReceiveString()
                .Result;

            var serializer = new JsonSerializer<ReactionResponseModel>();
            var response = serializer.DeserializeFromString(responseString);

            if (!response.ok)
            {
                throw new SlackApiException(response.error);
            }

            return response.ok;
        }

        public bool AddReaction(string slackApiToken, string reaction, string channel, double timestamp)
        {
            var responseString = "https://slack.com/api/reactions.add"
                .PostUrlEncodedAsync(
                    new
                    {
                        token = slackApiToken,
                        name = reaction,
                        channel = channel,
                        timestamp = timestamp
                    })
                .ReceiveString()
                .Result;

            var serializer = new JsonSerializer<ReactionResponseModel>();
            var response = serializer.DeserializeFromString(responseString);

            if (!response.ok)
            {
                throw new SlackApiException(response.error);
            }

            return response.ok;
        }
    }
}
