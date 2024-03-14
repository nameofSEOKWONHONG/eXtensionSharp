using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace eXtensionSharp.test {
    public class XFileTest
    {
        private string _filename;
            
        [SetUp]
        public void setup()
        {
            _filename = @"D:\test\test\test.txt";
            if (_filename.xExists().xIsFalse())
            {
                _filename.xFileCreateAll();
            }
        }
        
        [Test]
        public void create_file_all() {
            //tested only windows.
            Assert.IsTrue(_filename.xExists());
        }

        [Test]
        public void unique_file_test()
        {
            var result = _filename.xUniqueId();
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void file_lock_test()
        {
            _filename.xWriteFile("test");
            _filename.xLock();
            _filename.xUnLock();
            var result = _filename.xRead();
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void file_extension_test()
        {
            // var file = "d:\\\\D2 - 복사본.zip";
            //
            // var properties = file.xGetFileExtensionProperties();
            // if (properties != null)
            // {
            //     
            // }
            Assert.Pass();
        }
    }
}
