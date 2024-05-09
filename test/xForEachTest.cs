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
            var expected = 114999;
            var ranges = Enumerable.Range(1, expected).ToList();
            var isFind = false;
            ranges.xForEach(item => { });
            ranges.xForEach((index, item) =>
            {
                if(isFind.xIsFalse())
                {
                    isFind = item == expected;
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
            Enumerable.Range(1, 5000).xForEach(item => {
                list.Add(item);
            });

            Assert.That(list.Count, Is.EqualTo(5000));
            Assert.That(list[2], Is.EqualTo(3));
        }

        [Test]
        public async Task pararell_foreach_async_test()
        {
            var ranges = Enumerable.Range(1, 10).ToList();
            var canceled = await ranges
                .xParallelForEachAsync(null, async (item, token) =>
            {
                await Task.Factory.StartNew(() => TestContext.WriteLine(item), token);
            });
            Assert.That(canceled, Is.False);

            TestContext.WriteLine("===================================");

            var cts = new CancellationTokenSource();
            canceled = await ranges.xParallelForEachAsync(new ParallelOptions()
                {
                    MaxDegreeOfParallelism = 4,
                    CancellationToken = cts.Token
                }
                , async (item, token) =>
            {
                if (item == 1)
                {
                    await cts.CancelAsync();
                }

                await Task.Factory.StartNew(() => TestContext.WriteLine(item), token);
            });

            Assert.That(canceled, Is.True);
        }

        [Test]
        public async Task async_no_wait_test()
        {
            var task = Task.Factory.StartNew(() =>
            {
                TestContext.WriteLine("test");
            });
            await task.WaitAsync(TimeSpan.FromSeconds(1));
            var list = Enumerable.Range(1, 5000).ToList();
            list.xForEach(item =>
            {
                Assert.That(item,Is.Not.Zero);
            });
        }
    }
}