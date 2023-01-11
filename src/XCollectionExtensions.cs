using Mapster;
using System;
using System.Collections.Generic;

namespace eXtensionSharp
{
    public static class XCollectionExtensions
    {
        /// <summary>
        /// 모델간 값 자동 바인딩 (UI 갱신에 사용한다.), 계층구조 지원 안함.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        public static void xValueChange<T1>(this T1 src, T1 dest)
        {
            var props = src.xGetProperties();
            props.xForEach(item =>
            {
                var v = GetPropertyValue(src, item.Name);
                SetPropertyValue(dest, item.Name, v);
            });
        }

        private static object GetPropertyValue(object obj, string propertyName)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName);
            return propertyInfo?.GetValue(obj);
        }

        private static void SetPropertyValue(object obj, string propertyName, object value)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName);
            propertyInfo?.SetValue(obj, value);
        }

        /// <summary>
        /// <summary>
        /// 객체간 값 복사 (UI 갱신용 아님), 깊은 복사용
        /// </summary>
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T1 xMapping<T1>(this T1 src)
            where T1 : class, new()
        {
            return src.Adapt<T1>();
        }

        /// <summary>
        /// 객체간 값 복사 (UI 갱신용 아님)
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="src"></param>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public static T2 xMapping<T1, T2>(this T1 src, Func<T1, T2, T2> mapping = null)
            where T1 : class
            where T2 : class, new()
        {
            var mapped = src.Adapt<T2>();
            if (mapping.xIsNotEmpty())
            {
                return mapping(src, mapped);
            }
            return mapped;
        }

        /// <summary>
        /// 객체간 값 복사 (UI 갱신용 아님)
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public static T2 xMapping<T1, T2>(this T1 src, T2 dest, Func<T1, T2, T2> mapping = null)
            where T1 : class
            where T2 : class
        {
            var mapped = src.Adapt<T1, T2>(dest);
            if (mapping.xIsNotEmpty())
            {
                return mapping(src, mapped);
            }
            return mapped;
        }

        /// <summary>
        /// 객체간 값 복사 (UI 갱신용 아님)
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="src"></param>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public static IEnumerable<T2> xMapping<T1, T2>(this IEnumerable<T1> src, Func<T1, T2, T2> mapping)
            where T1 : class
            where T2 : class, new()
        {
            var newList = new List<T2>();
            foreach (var item in src)
            {
                var result = item.xMapping(mapping);
                newList.Add(result);
            }
            return newList;
        }

        /// <summary>
        /// 객체간 값 복사 (UI 갱신용 아님)
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="src"></param>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public static ICollection<T2> xMapping<T1, T2>(this ICollection<T1> src, Func<T1, T2, T2> mapping)
        {
            var newList = new List<T2>();
            foreach (var item in src)
            {
                var mapped = item.Adapt<T1, T2>();
                var result = mapping(item, mapped);
                newList.Add(result);
            }
            return newList;
        }

        /// <summary>
        /// 객체간 값 복사 (UI 갱신용 아님), Inner Class는 복사안됨.
        /// 계증구조는 지원안함.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IEnumerable<IDictionary<string, object>> xToDictionary<T>(this IEnumerable<T> values) where T : class
        {
            var list = new List<IDictionary<string, object>>();
            foreach (var value in values)
            {
                var item = value.xToDictionary<T>();
                list.Add(item);
            }
            return list;
        }
    }
}