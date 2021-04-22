using System;
using System.Linq;

namespace eXtensionSharp {
    public static class XReflection {
        public static TValue xGetAttrValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute {
            var att = type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            if (att.xIsNotNull()) return valueSelector(att);
            return default;
        }
    }
}