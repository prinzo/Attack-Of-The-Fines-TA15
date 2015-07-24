namespace FineBot.API.MemberInfo
{
    public class MemberModel
    {
         public string id { get; set; }
         public string name { get; set; }
         public bool deleted { get; set; }

        public ProfileModel profile { get; set; }

    }
}