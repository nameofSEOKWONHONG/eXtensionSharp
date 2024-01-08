using System;
using NUnit.Framework;

namespace eXtensionSharp.test
{
    public class EnumTest
    {
        /// <summary>
        /// abort
        /// </summary>
        // [Test]
        public void enum_static_class_test()
        {
            var v = "Y".xValue(ENUM_USE_YN.N.xValue<string>());
            Assert.AreEqual(ENUM_USE_YN.Y.xValue<string>(), "Y");
            Assert.That(ENUM_USE_YN.Y.ToString(), Is.Not.EqualTo(ENUM_USE_YN.N.ToString()));
        }

        [Test]
        public void enum_attribute_test()
        {
            Assert.AreEqual(EnumUseYn.Y.xEnumToString(), "Y");
        }

        /// <summary>
        /// abort
        /// </summary>
        // [Test]
        public void enum_attribute_test2()
        {
            Assert.AreEqual("Y".xValue<EnumUseYn>(EnumUseYn.Y), EnumUseYn.Y);
        }
    }

    public class ENUM_USE_YN : XEnumBase<ENUM_USE_YN>
    {
        public static readonly ENUM_USE_YN Y = Define("Y");
        public static readonly ENUM_USE_YN N = Define("N");
    }

    public enum EnumUseYn
    {
        [XEnumStringValue("Y")]
        Y,

        [XEnumStringValue("N")]
        N
    }
}