using System;

namespace FineBot.API.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dateTime)
        {
            var difference = 0;
            if (dateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                difference = (int) DayOfWeek.Monday - 7;
            }
            else
            {
                difference = DayOfWeek.Monday - dateTime.DayOfWeek;
            }
            
            return dateTime.AddDays(difference).Date;
        }

        public static DateTime StartOfMonth(this DateTime dateTime)
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, 1);
        }

        public static DateTime StartOfYear(this DateTime dateTime)
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, 1, 1);
        }
    }
}
