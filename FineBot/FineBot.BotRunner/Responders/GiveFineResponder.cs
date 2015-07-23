using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Models;
using MargieBot.Responders;

namespace FineBot.BotRunner.Responders
{
    public class GiveFineResponder : IGiveFineResponder
    {
        public bool CanRespond(ResponseContext context)
        {
            return context.Message.MentionsBot 
                && !context.BotHasResponded 
                && context.Message.Text.ToLower().Contains("fine");
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            Regex usernameRegex = new Regex(@"<@(?<username>\w+?)>", RegexOptions.Compiled);
            
            List<string> usernames = new List<string>();
            
            foreach (Match matchedUsername in usernameRegex.Matches(context.Message.Text))
            {
                usernames.Add(matchedUsername.Value);
            }

            if(usernames.Count == 1) return new BotMessage{Text = "I can't do that Dave"};

            List<string> cleanUsernames = new List<string>();

            foreach(var username in usernames)
            {
                if (username.Contains(context.BotUserID)) continue;

                cleanUsernames.Add(username);
            }

            var botMessage = new BotMessage();

            string multiple = String.Empty;
            if(cleanUsernames.Count > 1)
            {
                multiple = "s";
            }

            botMessage.Text = String.Format("Fine{1} awarded to {0}!", String.Join(", ", cleanUsernames), multiple);
            
            return botMessage;
        }
    }
}