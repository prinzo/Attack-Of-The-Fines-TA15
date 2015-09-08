using System.Collections.Generic;
using FineBot.Abstracts;

namespace FineBot.DataAccess.DataModels
{
    public class UserDataModel : GuidIdentifiedEntity
    {
        public UserDataModel()
        {
            this.Fines = new List<FineDataModel>();
        }

        public string EmailAddress { get; set; }
        public string SlackId { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public List<FineDataModel> Fines { get; set; }

    }
}
