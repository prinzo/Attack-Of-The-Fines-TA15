using System;

namespace FineBot.Errors
{
    public class SlackApiException : Exception
    {
        public SlackApiException(string message) : base(message)
        {  
        }
    }
}
