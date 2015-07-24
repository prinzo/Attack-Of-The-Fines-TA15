namespace FineBot.API.MemberInfo
{
    public interface IMemberInfoApi
    {
        MembersResponse GetAllMemberInformation();

        MemberModel GetMemberInformation(string slackId);
    }
}