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

        [Test]
        public void unique_file_test()
        {
            var file = "D:\\오류테스트폴더.zip";
            Console.WriteLine(file.xFileUniqueId());
        }

        [Test]
        public void file_lock_test()
        {
            var file = "D:\\오류테스트폴더.zip";
            file.xFileLock();
        }

        [Test]
        public void file_extension_test()
        {
            var file = "d:\\\\D2 - 복사본.zip";

            var properties = file.xGetFileExtensionProperties();
            if (properties != null)
            {
                
            }
        }
    }
}
