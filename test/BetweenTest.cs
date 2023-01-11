using System;
using NuGet.Frameworks;
using NUnit.Framework;

namespace eXtensionSharp.test
{
    public class BetweenTest
    {
        [Test]
        public void DateTimeBetweenTest()
        {
            var value = DateTime.Now;
            var from = DateTime.Now.AddDays(-1);
            var to = DateTime.Now.AddDays(1);

            Assert.IsTrue(value.xIsBetween(from, to));
            
            value = DateTime.Now.AddDays(2);
            Assert.IsFalse(value.xIsBetween(from, to));            
        }

        [Test]
        public void TimeSpanBetweenTest()
        {
            var value = DateTime.Now.TimeOfDay;
            var from = TimeSpan.Parse("13:27");
            var to = TimeSpan.Parse("14:27");
            Assert.IsTrue(value.xIsBetween(from, to));

            value = TimeSpan.Parse("14:28");
            Assert.IsFalse(value.xIsBetween(from, to));
        }

        [Test]
        public void CharBetweenTest()
        {
            var value = 'B';
            var from = 'A';
            var to = 'C';

            Assert.IsTrue(value.xIsBetween(from, to));
            value = 'D';
            Assert.IsFalse(value.xIsBetween(from, to));
            value = 'b';
            Assert.IsFalse(value.xIsBetween(from, to));
            Assert.IsTrue(value.xIsBetween(from, to, false));            
        }
    }
}