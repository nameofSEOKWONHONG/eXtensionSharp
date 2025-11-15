using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace eXtensionSharp.test
{
    public class BetweenTest
    {
        [Test]
        public void datetime_between_test()
        {
            var value = DateTime.Now;
            var from = DateTime.Now.AddDays(-1);
            var to = DateTime.Now.AddDays(1);

            Assert.That(value.xIsBetween(from, to), Is.True);
            
            value = DateTime.Now.AddDays(2);
            Assert.That(value.xIsBetween(from, to), Is.False);            
        }

        [Test]
        public void TimeSpanBetweenTest()
        {
            var value = DateTime.Now.TimeOfDay;
            var from = DateTime.Now.AddMinutes(-1).TimeOfDay;
            var to = DateTime.Now.AddMinutes(1).TimeOfDay;
            Assert.That(value.xIsBetween(from, to), Is.True);
        }

        [Test]
        public void char_between_test()
        {
            var value = 'B';
            var from = 'A';
            var to = 'C';

            Assert.That(value.xIsBetween(from, to), Is.True);
            value = 'D';
            Assert.That(value.xIsBetween(from, to), Is.False);
            value = 'b';
            Assert.That(value.xIsBetween(from, to), Is.False);       
        }

        [Test]
        public void duplicate_out_val_test()
        {
            string[] arr = new[] { "A", "B", "C", "C"};
            if (arr.xTryDuplicate(out var v))
            {
                Assert.That(v, Is.EqualTo("C"));
            }
        }
    }

    public class Sample
    {
        public string Name { get; set; }
    }
}