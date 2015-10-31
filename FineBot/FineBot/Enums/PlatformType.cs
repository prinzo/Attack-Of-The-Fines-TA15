using System.ComponentModel;

namespace FineBot.Enums
{
    public enum PlatformType
    {
        [Description("Web front end")]
        WebFrontEnd = 1,
        [Description("Slack")]
        Slack = 2
    }
}