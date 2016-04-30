namespace FineBot.API.MemberInfo
{
    public class UserInfoResponse
    {
        public bool ok { get; set; }
        public string error { get; set; }
        public MemberModel user { get; set; }
    }
}