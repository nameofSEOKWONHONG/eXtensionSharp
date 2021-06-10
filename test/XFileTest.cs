using System;
using System.IO;
using NUnit.Framework;

namespace eXtensionSharp.test {
    public class XFileTest {
        [Test]
        public void create_file_all() {
            //tested only windows.
            var fileName = @"D:\test\test\test.txt";
            fileName.xFileCreateAll();
            
            Assert.IsTrue(File.Exists(fileName));
        }
    }
}
