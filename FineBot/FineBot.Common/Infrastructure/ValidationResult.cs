using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FineBot.Common.Enums;

namespace FineBot.Common.Infrastructure
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            this.ValidationMessages = new List<ValidationMessage>();
        }

        public List<ValidationMessage> ValidationMessages;

        public string FullTrace
        {
            get
            {
                return this.GetMessages(x => true);
            }
        }

        public bool HasErrors 
        {
            get
            {
                return this.ValidationMessages.Any(x => x.Severity == Severity.Error);
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

        public ValidationResult AddMessage(Severity severity, string message)
        {
            if(this.ValidationMessages == null)
            {
                this.ValidationMessages = new List<ValidationMessage>();
            }

            this.ValidationMessages.Add(new ValidationMessage(message, severity));

            return this;
        }
    }
}