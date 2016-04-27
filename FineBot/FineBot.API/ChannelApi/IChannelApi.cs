using System;
using FineBot.API.GroupsApi;

namespace FineBot.API.ChannelApi
{
    public interface IChannelApi
    {
        ChannelListResponseModel ListChannels(string slackApiToken);

        GroupsHistoryResponseModel GetChannelHistory(string slackApiToken, string channel, DateTime startTime, DateTime endTime, int count);

        ChannelInfoResponseModel GetChannelInfo(string slackApiToken, string channel);
    }
}
