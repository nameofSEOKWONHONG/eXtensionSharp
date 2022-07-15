using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace eXtensionSharp.test;

public class ClonePerformanceTest
{
    [Test]
    public void clone_performace_test1()
    {
        var list = new List<PararellData>();
        Enumerable.Range(1, 1000).ToList().xForEach(item =>
        {
            list.Add(new PararellData()
            {
                COM_CODE = (1 + item).ToString(), CONTENTS = "test" + item.ToString(), FORM_SEQ = item, FORM_TYPE = "SI030"
            });
        });

        var src = list.First();
        var clone = src.xToClone();
        src.FORM_SEQ += 1;
        Console.WriteLine(src.FORM_SEQ);
        clone.FORM_SEQ += 1;
        Console.WriteLine(clone.FORM_SEQ);
    }
}