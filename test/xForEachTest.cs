using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace eXtensionSharp.test {
    public class xForEachTest {
        [Test]
        public void datetime_foreach_test() {
            // var from = DateTime.Parse("2020-01-01");
            // var to = DateTime.Parse("2021-12-31");
            //
            // var list = new List<DateTime>();
            // (from, to).xForEach(ENUM_DATETIME_FOREACH_TYPE.DAY, o => {
            //     list.Add(o);
            // });
            //
            // Assert.AreEqual(list.xFirst(), DateTime.Parse("2020-01-01"));
            // Assert.AreEqual(list.xLast(), DateTime.Parse("2021-12-31"));
        }

        [Test]
        public void datetime_foreach_month_test() {
            // var from = DateTime.Parse("2020-01-01");
            // var to = DateTime.Parse("2021-12-31");
            //
            // var list = new List<DateTime>();
            // (from, to).xForEach(ENUM_DATETIME_FOREACH_TYPE.MONTH, o => {
            //     list.Add(o);
            // });
            //
            // Assert.AreEqual(list.xFirst(), DateTime.Parse("2020-01-01"));
            // Assert.AreEqual(list.xLast(), DateTime.Parse("2021-12-01"));
            //
            // Assert.GreaterOrEqual(list.Count, 12 * 2);
        }

        [Test]
        public void datetime_foreach_year_test() {
            // var from = DateTime.Parse("2020-01-01");
            // var to = DateTime.Parse("2021-12-31");
            //
            // var list = new List<DateTime>();
            // (from, to).xfore(ENUM_DATETIME_FOREACH_TYPE.YEAR, o => {
            //     list.Add(o);
            // });
            //
            // Assert.AreEqual(list.xFirst(), DateTime.Parse("2020-01-01"));
            // Assert.AreEqual(list.xLast(), DateTime.Parse("2021-01-01"));
            //
            // Assert.GreaterOrEqual(list.Count, 2);
        }

        [Test]
        public void number_from_to_test() {
            (1, 10).xForEach(num => {
                Console.WriteLine(num);
            });
        }

        [Test]
        public void reverse_number_from_to_test() {
            (1, 10).xForEachReversal(i => {
                Console.WriteLine(i);
            });
        }

        [Test]
        public async Task foreach_async_test()
        {
            // await Enumerable.Range(1, 100).ToList().xForEachAsync(async v =>
            // {
            //     await simple(v);
            // });
            //
            // Console.WriteLine("===================================================");

            await Enumerable.Range(2, 20).ToList().xForEachParallelAsync(
                async (v, token) =>
                {
                    await simple(v);
                }
                , new ParallelOptions() { MaxDegreeOfParallelism = 5 }
                );
        }

        [Test]
        public async Task xforeach_async_test()
        {
            var nums = Enumerable.Range(1, 5001).ToList();
            var result = 0;
            await nums.xForEachAsync(item =>
            {
                result += 1;
            });
            Assert.NotZero(result);
        }

        public async Task simple(int v)
        {
            await Task.Delay(100);
            Console.WriteLine(v);
        }
    }
}