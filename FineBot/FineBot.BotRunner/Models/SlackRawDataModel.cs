﻿namespace FineBot.BotRunner.Models
{
    public class SlackRawDataModel
    {
        public string type { get; set; }
        public string channel { get; set; }
        public string user { get; set; }
        public string text { get; set; }
        public string ts { get; set; }
        public string team { get; set; }
    }
}
