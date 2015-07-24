using System.Configuration;
using Flurl.Http;
using ServiceStack.Text;

namespace FineBot.API.MemberInfo
{
    public class MemberInfoApi : IMemberInfoApi
    {
        public MembersResponse GetAllMemberInformation()
        {
            var responseString = "https://slack.com/api/users.list"
                                .PostUrlEncodedAsync(new { token = ConfigurationManager.AppSettings["SlackApiKey"] })
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
                                                         token = ConfigurationManager.AppSettings["SlackApiKey"],
                                                         user = slackId
                                                     })
                                .ReceiveString().Result;

            var serializer = new JsonSerializer<UserInfoResponse>();
            var memberInfo = serializer.DeserializeFromString(responseString);

            return memberInfo.user;
        }
    }
}