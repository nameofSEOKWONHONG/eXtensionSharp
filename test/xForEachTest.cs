using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using NUnit.Framework;

namespace eXtensionSharp.test {
    public class xForEachTest {
        [Test]
        public void xforeach_test()
        {
            var ranges = Enumerable.Range(1, 5001).ToList();
            var isFind = false;
            ranges.xForEach((index, item) =>
            {
                if(isFind.xIsFalse())
                {
                    isFind = item == 5001;
                }
            });
            Assert.IsTrue(isFind);
        }

        [Test]
        public async Task xforeach_async_test()
        {
            var ranges = Enumerable.Range(1, 5001).ToList();
            var isFind = false;
            await ranges.xForEachAsync(async item =>
            {
                await Task.Run(() => {
                    if (isFind.xIsFalse())
                    {
                        isFind = item == 2000;
                    }
                });

            });
            Assert.IsTrue(isFind);
        }

        [Test]
        public void datetime_from_to_foreach_test() {
            var from = DateTime.Parse("2020-01-01");
            var to = DateTime.Parse("2021-12-31");
            var expected = "2020-12-31";

            var findDate = string.Empty;
            (from, to).xForEach(date =>
            {
                if(date.ToString("yyyy-MM-dd") == expected)
                {
                    findDate = date.ToString("yyyy-MM-dd");
                    return false;
                }

                return true;
            });
            Assert.That(findDate, Is.EqualTo(expected));
        }

        [Test]
        public void number_from_to_test() {
            var list = new List<int>();
            (1, 5001).xForEach(num => {
                list.Add(num);
            });

            Assert.That(list.Count, Is.EqualTo(5001));
            Assert.That(list[2], Is.EqualTo(3));
        }

        [Test]
        public async Task pararell_foreach_async_test()
        {
            var ranges = Enumerable.Range(1, 10).ToList();
            var canceled = await ranges.xParallelForEachAsync(async (item, token) =>
            {
                await Task.Factory.StartNew(() => TestContext.WriteLine(item));
            });
            Assert.That(canceled, Is.False);

            TestContext.WriteLine("===================================");

            var cts = new CancellationTokenSource();
            canceled = await ranges.xParallelForEachAsync(async (item, token) =>
            {
                if (item == 1)
                {
                    await cts.CancelAsync();
                }

                await Task.Factory.StartNew(() => TestContext.WriteLine(item));

            }, new ParallelOptions() { CancellationToken = cts.Token, MaxDegreeOfParallelism = 2 });

            Assert.That(canceled, Is.True);
        }
    }
}