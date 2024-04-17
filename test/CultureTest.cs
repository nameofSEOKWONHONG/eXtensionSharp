using NUnit.Framework;

namespace eXtensionSharp.test;

public class CultureTest
{
    [Test]
    public void language_display_name_test()
    {
        var expected = "영어(미국)";
        var result = "en-US".xToLanguageDisplayName();
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void language_english_name_test()
    {
        var expected = "English (United States)";
        var result = "en-US".xToLanguageEngName();
        Assert.That(result, Is.EqualTo(expected));
    }
}