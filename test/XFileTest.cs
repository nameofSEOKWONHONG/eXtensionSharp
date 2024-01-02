using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace eXtensionSharp.test {
    public class XFileTest 
    {
        string _filename = "D:\\test\\test\\test.txt"; 

        [SetUp]
        public void Setup()
        {
            _filename.xFileCreateAll();
            _filename.xFileWrite("test", Encoding.UTF8);
        }
        
        [Test]
        public void create_file_all() {
            Assert.IsTrue(File.Exists(_filename));
        }

        [Test]
        public void unique_file_test()
        {
            Assert.That(_filename.xFileUniqueId().xIsNotEmpty(), Is.True);
        }

        [Test]
        public void file_lock_test()
        {
            _filename.xFileLock();
            _filename.xFileUnLock();
        }

        // [Test]
        // public void file_extension_test()
        // {
        //     // var properties = _filename.xGetFileExtensionProperties();
        //     // Assert.That(properties.xIsNotEmpty(), Is.True);
        // }

        [Test]
        public void file_read_to_line_test()
        {
            var destPath = $@"D:\{Guid.NewGuid()}.txt";
            File.Copy(_filename, destPath);
            
            var lines = destPath.xFileReadAllLines();
            TestContext.Out.WriteLine(lines.xJoin());
            Assert.IsTrue(lines.xIsNotEmpty());
        }
    }
}
