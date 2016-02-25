using System;
using FineBot.API.Extensions;
using FineBot.API.Models;

namespace FineBot.API.ChatApi
{
    public class PostMessageResponseModel
    {
        public bool ok { get; set; }
        public string error { get; set; }
        public double ts { get; set; }
        public string channel { get; set; }
        public Message message { get; set; }

        public DateTime TimeStamp
        {
            get { return ts.ToLocalDateTime(); }
        }
    }
}
