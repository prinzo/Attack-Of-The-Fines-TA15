using System;

namespace FineBot.API.UsersApi
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string SlackId { get; set; }
        public string EmailAddress { get; set; }
        public int FineCount { get; set; }
    }
}