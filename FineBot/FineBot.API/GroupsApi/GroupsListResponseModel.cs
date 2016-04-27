using FineBot.API.Models;

namespace FineBot.API.GroupsApi
{
    public class GroupsListResponseModel
    {
        public bool ok { get; set; }
        public Group[] groups { get; set; }
        public string error { get; set; }
    }
}
