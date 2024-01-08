using System;
using System.Collections.Generic;
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
            var from = DateTime.Now.AddMinutes(-1).TimeOfDay;
            var to = DateTime.Now.AddMinutes(1).TimeOfDay;
            Assert.IsTrue(value.xIsBetween(from, to));
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

        [Test]
        public void duplicate_test()
        {
            string[] arr = new[] { "A", "B", "C"};
            var expected = arr.xIsDuplicate();
            Assert.IsFalse(expected);

            List<Sample> arr2 = new List<Sample>();
            arr2.Add(new Sample() { Name = "A" });
            arr2.Add(new Sample() { Name = "B" });
            arr2.Add(new Sample() { Name = "C" });
            arr2.Add(new Sample() { Name = "A" });

            var expected2 = arr2.xSelect(m => m.Name).xIsDuplicate();
            Assert.IsTrue(expected2);

        }
    }

    public class Sample
    {
        public string Name { get; set; }
    }
}