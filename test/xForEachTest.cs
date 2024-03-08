using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            await ranges.xForEachAsync(item =>
            {
                if (isFind.xIsFalse())
                {
                    isFind = item == 2000;
                }
            });

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
        public void datetime_foreach_test() {
            var from = DateTime.Parse("2020-01-01");
            var to = DateTime.Parse("2021-12-31");

            var list = new List<DateTime>();            
            var isFind = true;
            (from, to).xForEach(date =>
            {
                if (isFind.xIsFalse())
                {
                    isFind = date.ToString("yyyy-MM-dd") == "2020-12-31";
                }
            });
            Assert.IsTrue(isFind);

            isFind = false;
            (from, to).xForEach(date =>
            {
                isFind = date.ToString("yyyy-MM-dd") == "2020-12-31";
                if (isFind) return false;

                return true;
            });
            Assert.IsTrue(isFind);
        }

        [Test]
        public void number_from_to_test() {
            (1, 10).xForEach(num => {
                Console.WriteLine(num);
            });
        }
    }
}