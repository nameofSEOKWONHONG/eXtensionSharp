using System;
using NUnit.Framework;

namespace eXtensionSharp.test {
    public class EnumTest {
        [Test]
        public void enum_static_class_test() {
            Assert.AreEqual(ENUM_USE_YN.Y.xSafe(), "Y");
            Assert.AreNotEqual(ENUM_USE_YN.N, ENUM_USE_YN.Y);
        }

        [Test]
        public void enum_attribute_test() {
            Assert.AreEqual(EnumUseYn.Y.xEnumToString(), "Y");
        }

        [Test]
        public void enum_attribute_test2() {
            Assert.AreEqual("Y".xSafe<EnumUseYn>(), EnumUseYn.Y);
        }
    }

    public class ENUM_USE_YN : XENUM_BASE<ENUM_USE_YN> {
        public static readonly ENUM_USE_YN Y = Define("Y");
        public static readonly ENUM_USE_YN N = Define("N");
    }

    public enum EnumUseYn {
        [XEnumStringValue("Y")]
        Y,
        [XEnumStringValue("N")]
        N
    }
}