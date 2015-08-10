using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FineBot.API.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dateTime)
        {
            int difference = DayOfWeek.Monday - dateTime.DayOfWeek;
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
