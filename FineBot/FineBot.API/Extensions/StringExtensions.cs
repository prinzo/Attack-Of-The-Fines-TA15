namespace FineBot.API.Extensions
{
    public static class StringExtensions
    {
        public static string FormatUserId(this string rawSlackUserId)
        {
            return string.Format("<@{0}>", rawSlackUserId);
        }

        public static string CleanSlackId(this string formattedSlackId)
        {
            return formattedSlackId.StartsWith("<") ? formattedSlackId.Substring(2, formattedSlackId.Length - 3) : formattedSlackId;
        }

        public static string LocalPart(this string email)
        {
            var index = email.IndexOf('@');
            return index == -1 ? email : email.Substring(0, index + 1);
        }
    }
}
