using NUnit.Framework;

namespace eXtensionSharp.test {
    public class XStringTest {
        [Test]
        public void split_test() {
            var chars ="a,b,c,d,e".xSplit(',');
            Assert.AreEqual(chars.xFirst(), "a");
            Assert.AreEqual(chars.xLast(), "e");
        }
    }
}