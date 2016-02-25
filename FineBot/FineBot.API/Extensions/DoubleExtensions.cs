using System;

namespace FineBot.API.Extensions
{
    public static class DoubleExtensions
    {
        public static DateTime ToLocalDateTime(this double utcSeconds)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(utcSeconds)
                    .ToLocalTime();
        }
    }
}
