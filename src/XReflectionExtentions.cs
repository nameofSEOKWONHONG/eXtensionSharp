using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace eXtensionSharp
{
    public static class XReflectionExtentions
    {
        public static TValue xGetAttrValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            if (!att.xIsEmpty()) return valueSelector(att);
            return default;
        }

        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> AssignPropertyInfoStates = new();
        public static IEnumerable<PropertyInfo> xGetProperties<T>(this T obj)
        {
            Type itemType = typeof(T);
            if (AssignPropertyInfoStates.TryGetValue(itemType, out PropertyInfo[] cachedResult))
            {
                return cachedResult;
            }
            var result = obj.GetType().GetProperties();
            AssignPropertyInfoStates.TryAdd(itemType, result);
            return result;
        }

        public static IEnumerable<T> xCreateInstance<T>(this string assemblyPath, string[] containKeywords = null, string[] notContainKeywords = null) 
            where T : class
        {
            var list = new List<T>();
            var assembly = Assembly.LoadFrom(assemblyPath);
            var types = assembly.GetTypes();
            types.xForEach(type =>
            {
                if (type.Name.xContains(containKeywords))
                {
                    if (type.Name.xContains(notContainKeywords)) return true; //continue;

                    if (type.IsClass && !type.IsAbstract && !type.IsInterface)
                        list.Add(Activator.CreateInstance(type) as T);
                }

                return true;
            });

            return list;
        }
    }
}