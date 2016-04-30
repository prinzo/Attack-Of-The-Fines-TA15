namespace FineBot.API.Extensions
{
    public static class StringExtensions
    {
        public static string FormatUserId(this string rawSlackUserId)
        {
            return string.Format("<@{0}>", rawSlackUserId);
        }
    }
}
