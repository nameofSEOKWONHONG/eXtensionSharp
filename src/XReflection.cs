using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public static IEnumerable<PropertyInfo> GetProperties<T>(this T obj) {
            return obj.GetProperties();
        }
        
        public static IEnumerable<T> CreateInstance<T>(this string assemblyPath, string[] containKeywords = null, string[] notContainKeywords = null) where T : class {
            var list = new XList<T>();
            Assembly assembly = Assembly.LoadFrom(assemblyPath);  
            var types = assembly.GetTypes();
            types.xForEach(type => {
                if (type.Name.xContains(containKeywords)) {
                    if (type.Name.xContains(notContainKeywords)) return true; //continue;

                    if (type.IsClass && !type.IsAbstract && !type.IsInterface) {
                        list.Add(Activator.CreateInstance(type) as T);
                    }
                }

                return true;
            });

            return list;
        }
    }
}