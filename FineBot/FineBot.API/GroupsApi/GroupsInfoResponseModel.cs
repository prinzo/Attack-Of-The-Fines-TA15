﻿using FineBot.API.Models;

namespace FineBot.API.GroupsApi
{
    public class GroupsInfoResponseModel
    {
        public bool ok { get; set; }
        public Group group { get; set; }
        public string error { get; set; }
    }
}
