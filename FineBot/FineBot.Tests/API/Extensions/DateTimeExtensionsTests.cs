using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FineBot.API.Extensions;

namespace FineBot.Tests.API.Extensions
{
    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [Test]
        [TestCase("2015-05-11", "2015-05-11")] // Monday
        [TestCase("2011-01-11", "2011-01-10")] // Tuesday
        [TestCase("2015-08-12", "2015-08-10")] // Wednesday
        [TestCase("2015-01-01", "2014-12-29")] // Thursday
        [TestCase("2044-03-04", "2044-02-29")] // Friday
        [TestCase("1975-07-19", "1975-07-14")] // Saturday
        [TestCase("1904-06-12", "1904-06-06")] // Sunday
        public void GivenADateTimeThenReturnThePreviousMondayDateTime(DateTime dateTime, DateTime answer)
        {
            var startOfWeek = dateTime.StartOfWeek();
            Assert.AreEqual(startOfWeek, answer);
        }
    }
}
