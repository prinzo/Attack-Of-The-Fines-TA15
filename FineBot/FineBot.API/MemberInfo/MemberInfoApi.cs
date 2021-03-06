﻿using System.Configuration;
using Flurl.Http;
using ServiceStack.Text;

namespace FineBot.API.MemberInfo
{
    public class MemberInfoApi : IMemberInfoApi
    {
        public MembersResponse GetAllMemberInformation()
        {
            var responseString = "https://slack.com/api/users.list"
                                .PostUrlEncodedAsync(new { token = ConfigurationManager.AppSettings["BotKey"] })
                                .ReceiveString().Result;

            var serializer = new JsonSerializer<MembersResponse>();
            var memberInfo = serializer.DeserializeFromString(responseString);

            return memberInfo;
        }

        public MemberModel GetMemberInformation(string slackId)
        {
            var responseString = "https://slack.com/api/users.info"
                                .PostUrlEncodedAsync(new
                                                     {
                                                         token = ConfigurationManager.AppSettings["BotKey"],
                                                         user = CleanSlackId(slackId)
                                                     })
                                .ReceiveString().Result;

            var serializer = new JsonSerializer<UserInfoResponse>();
            var memberInfo = serializer.DeserializeFromString(responseString);

            return memberInfo.user;
        }

        private static string CleanSlackId(string slackId)
        {
            return slackId.StartsWith("<") ? slackId.Substring(2, slackId.Length - 3) : slackId;
        }
    }
}