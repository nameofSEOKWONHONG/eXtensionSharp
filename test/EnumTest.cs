using System;
using Ardalis.SmartEnum;
using NUnit.Framework;

namespace eXtensionSharp.test
{
    public class EnumTest
    {
        [Test]
        public void enum_static_class_test()
        {
            var v = "Y".xValue(ENUM_USE_YN.N.Name.xValue<string>());
            Assert.That("Y", Is.EqualTo(ENUM_USE_YN.Y.Name.xValue<string>()));
            Assert.That(ENUM_USE_YN.Y, Is.Not.EqualTo(ENUM_USE_YN.N));
        }

        // [Test]
        // public void enum_attribute_test()
        // {
        //     // Assert.AreEqual(EnumUseYn.Y.xEnumToString(), "Y");
        // }

        [Test]
        public void enum_attribute_test2()
        {
            Assert.That(EnumUseYn.Y, Is.EqualTo("Y".xValue<EnumUseYn>(EnumUseYn.Y)));
        }
    }

    public class ENUM_USE_YN : SmartEnum<ENUM_USE_YN>
    {
        public static readonly ENUM_USE_YN Y = new ENUM_USE_YN("Y", 1);
        public static readonly ENUM_USE_YN N = new ENUM_USE_YN("N", 2);
        
        public ENUM_USE_YN(string name, int value) : base(name, value)
        {
        }
    }

    public enum EnumUseYn
    {
        Y,
        N
    }
}