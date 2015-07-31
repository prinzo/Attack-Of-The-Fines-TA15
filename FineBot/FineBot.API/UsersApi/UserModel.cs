using System;
using System.Collections.Generic;
using FineBot.API.FinesApi;

namespace FineBot.API.UsersApi
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string SlackId { get; set; }
        public string EmailAddress { get; set; }
        public string DisplayName { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mobile { get; set; }
        public int AwardedFineCount { get; set; }
        public int PendingFineCount { get; set; }
        public List<FineModel> Fines { get; set; }
    }
}