using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        [Test]
        public void file_read_to_line_test()
        {
            var filePath = @"D:\workspace\Triptopaz\src\03.WebApp\Triptopaz.Backend.Api\Logs\log20231101.txt";
            var destPath = $@"D:\{Guid.NewGuid()}.txt";
            
            File.Copy(filePath, destPath);
            
            var lines = destPath.xFileReadAllLines();
            TestContext.Out.WriteLine(lines.xJoin());
            Assert.IsTrue(lines.xIsNotEmpty());

            var errors = new List<string>();
            lines.xForEachReverse(line =>
            {
                if (line.Contains("[ERR]"))
                {
                    errors.Add(line);    
                }
                
                return true;
            });
            
            if (errors.xIsNotEmpty())
            {
                var error1 = errors[0];
                var error2 = errors[errors.Count - 1];
                    
                var dt1 = error1.Substring(0, 30).xToDate();
                var dt2 = error2.Substring(0, 30).xToDate();

                if ((dt1 - dt2).TotalMinutes <= 5 &&
                    errors.Count >= 100)
                {
                    //warning notification
                }
            }
        }
    }
}
