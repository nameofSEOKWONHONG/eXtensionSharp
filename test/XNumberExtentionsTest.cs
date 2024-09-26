using NUnit.Framework;

namespace eXtensionSharp.test;


public class XNumberExtentionsTest
{
    [Test]
    public void xto_13_length_phonenumber_test()
    {
        var text = "8201011112222";
        var number = text.xToPhoneNumber();
        Assert.That(number, Is.EqualTo("+82-010-1111-2222"));
    }

    [Test]
    public void xto_13_under_length_phonenumber_test2()
    {
        var text = "01011112222";
        var number = text.xToPhoneNumber();
        Assert.That(number, Is.EqualTo("010-1111-2222"));        
    }
}