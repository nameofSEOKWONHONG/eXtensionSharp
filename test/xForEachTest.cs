using System;
using NUnit.Framework;

namespace eXtensionSharp.test {
    public class xForEachTest {
        [Test]
        public void datetime_foreach_test() {
            var from = DateTime.Parse("2020-01-01");
            var to = DateTime.Parse("2021-12-31");

            var list = new XList<DateTime>();
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

            var list = new XList<DateTime>();
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

            var list = new XList<DateTime>();
            (from, to).xForEach(ENUM_DATETIME_FOREACH_TYPE.YEAR, o => {
                list.Add(o);
            });
            
            Assert.AreEqual(list.xFirst(), DateTime.Parse("2020-01-01"));
            Assert.AreEqual(list.xLast(), DateTime.Parse("2021-01-01"));
            
            Assert.GreaterOrEqual(list.Count, 2);
        }
    }
}