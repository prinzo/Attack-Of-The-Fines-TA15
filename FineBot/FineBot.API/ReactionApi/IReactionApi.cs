using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FineBot.API.ReactionApi
{
    public interface IReactionApi
    {
        bool AddReaction(string slackApiToken, string reaction, string channel, string timestamp);
        bool AddReaction(string slackApiToken, string reaction, string channel, double timestamp);
    }
}
