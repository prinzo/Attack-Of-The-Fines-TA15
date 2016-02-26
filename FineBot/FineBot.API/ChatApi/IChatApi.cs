using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FineBot.API.ChatApi
{
    public interface IChatApi
    {
        PostMessageResponseModel PostMessage(string slackApiToken, string channel, string text);
    }
}
