﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace eXtensionSharp.test {
    public class XFileTest
    {
        [SetUp]
        public void setup()
        {

        }

        [Test]
        public void file_test()
        {
			var filename = @"D:\test\test\test\test.txt";
            if (filename.xExists()) filename.xDeleteAll();

			var content = "hello world!";
			var isException = false;

			try
			{
				filename.xWrite(() => content.xToBytes());
			}
			catch
			{
				isException = true;
			}

			Assert.IsTrue(isException);

			filename.xWriteAll(() => content.xToBytes());
            var text = filename.xRead();
            Assert.That(content, Is.EqualTo(text));

			var fileName = filename.xGetFileName();
			Assert.That(fileName, Is.EqualTo("test.txt"));

			var extension = filename.xGetExtension();
			Assert.That(extension, Is.EqualTo(".txt"));
		}
    }
}
