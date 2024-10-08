using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eXtensionSharp.Job;
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
            var ranges = Enumerable.Range(1, 100).ToList();
            var value = 0;
            await ranges.xForEachAsync(Process);
            //Assert.That(value, Is.Not.Zero);
        }

        private async Task Process(int i)
        {
            TestContext.WriteLine(i);
            await Task.Delay(1);
            TestContext.WriteLine(i + "complete");
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

        [Test]
        public void xbatch_test()
        {
            var items = Enumerable.Range(1, 1000).ToList();
            var batchItems = items.xBatch(412);
            foreach (var batchItem in batchItems)
            {
                Assert.That(items, Does.Contain(batchItem.First()));
            }

            Assert.Pass();
        }

        [Test]
        public void xforeach_product_test()
        {
            var items1 = Enumerable.Range(1, 3).ToArray();
            var items2 = Enumerable.Range(4, 3).ToArray();
            var items3 = Enumerable.Range(7, 3).ToArray();
            
            items1.xForEach(items2, items3, (a, b, c) =>
            {
                Assert.That(a, Is.EqualTo(items1[a - 1]));
                Assert.That(b, Is.EqualTo(items2[b - 4]));
                Assert.That(c, Is.EqualTo(items3[c - 7]));
            });
        }

        [Test]
        public async Task processor_test()
        {
            JobProsessor<int>.Instance.SetProessor(JobHandler<int>.Instance);
            
            JobHandler<int>.Instance.Enqueue(1);
            JobHandler<int>.Instance.Enqueue(2);
            JobHandler<int>.Instance.Enqueue(3);
            JobHandler<int>.Instance.Enqueue(4);

            await Task.Delay(5000 * 2);
        }
    }
}