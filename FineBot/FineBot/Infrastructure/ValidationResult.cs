using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FineBot.Enums;

namespace FineBot.Infrastructure
{
    public class ValidationResult
    {
        public List<ValidationMessage> ValidationMessages;

        public string FullTrace
        {
            get
            {
                return this.GetMessages(x => true);
            }
        }

        public string GetMessages(Predicate<Severity> predicate)
        {
            var messages = this.ValidationMessages.Where(x => predicate(x.Severity));

            StringBuilder sb = new StringBuilder();
            
            foreach (var validationMessage in messages)
            {
                sb.AppendLine(validationMessage.ToString());
            }

            return sb.ToString();
        }
    }
}