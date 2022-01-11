using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace eXtensionSharp.test {
    public class xForEachTest {
        [Test]
        public void datetime_foreach_test() {
            var from = DateTime.Parse("2020-01-01");
            var to = DateTime.Parse("2021-12-31");

            var list = new List<DateTime>();
            (from, to).xForEach(ENUM_DATETIME_FOREACH_TYPE.DAY, o => {
                list.Add(o);
            });
            
            Assert.AreEqual(list.xFirst(), DateTime.Parse("2020-01-01"));
            Assert.AreEqual(list.xLast(), DateTime.Parse("2021-12-31"));
        }

        [Test]
        public void datetime_foreach_month_test() {
            var from = DateTime.Parse("2020-01-01");
            var to = DateTime.Parse("2021-12-31");

            var list = new List<DateTime>();
            (from, to).xForEach(ENUM_DATETIME_FOREACH_TYPE.MONTH, o => {
                list.Add(o);
            });
            
            Assert.AreEqual(list.xFirst(), DateTime.Parse("2020-01-01"));
            Assert.AreEqual(list.xLast(), DateTime.Parse("2021-12-01"));
            
            Assert.GreaterOrEqual(list.Count, 12 * 2);
        }

        [Test]
        public void datetime_foreach_year_test() {
            var from = DateTime.Parse("2020-01-01");
            var to = DateTime.Parse("2021-12-31");

            var list = new List<DateTime>();
            (from, to).xForEach(ENUM_DATETIME_FOREACH_TYPE.YEAR, o => {
                list.Add(o);
            });
            
            Assert.AreEqual(list.xFirst(), DateTime.Parse("2020-01-01"));
            Assert.AreEqual(list.xLast(), DateTime.Parse("2021-01-01"));
            
            Assert.GreaterOrEqual(list.Count, 2);
        }

        [Test]
        public void number_from_to_test() {
            (1, 10).xForEach(num => {
                Console.WriteLine(num);
            });
        }

        [Test]
        public void reverse_number_from_to_test() {
            (1, 10).xForEachReverse(i => {
                Console.WriteLine(i);
            });
        }
    }
}