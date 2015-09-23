﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FineBot.BotRunner.Extensions
{
    public static class StringExtensions
    {
        public static bool IsYouTubeLink(this string text)
        {
            var youtubeRegex = new Regex(@"(youtube.com\/watch\?|youtu.be\/)", RegexOptions.Compiled);
            return youtubeRegex.IsMatch(text);
        }
    }
}