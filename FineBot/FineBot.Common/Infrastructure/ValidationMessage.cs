using System;
using FineBot.Common.Enums;
using FineBot.Common.ExtensionMethods;

namespace FineBot.Common.Infrastructure
{
    public class ValidationMessage
    {
        public ValidationMessage(string message, Severity severity)
        {
            this.Message = message;
            this.Severity = severity;
        }

        public override string ToString()
        {
            return String.Format("[{0}] - {1}", this.Severity.ToDescription(), this.Message);
        }

        public string Message { get; set; }
        public Severity Severity { get; set; }
    }
}