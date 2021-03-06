﻿using System;
using FineBot.API.Extensions;
using FineBot.API.Models;

namespace FineBot.API.GroupsApi
{
    public class GroupsHistoryResponseModel
    {
        public GroupsHistoryResponseModel()
        {
            messages = new Message[0];
        }

        public bool ok { get; set; }
        public double latest { get; set; }
        public Message[] messages { get; set; }
        public bool has_more { get; set; }
        public bool is_limited { get; set; }
        public string error { get; set; }

        public DateTime LatestTimeStamp
        {
            get { return latest.ToLocalDateTime(); }
        }
    }
}
