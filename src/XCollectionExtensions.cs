using Mapster;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

namespace eXtensionSharp
{
    public class DataTypeName
    {
        public static string Int16 = typeof(Int16).ToString();
        public static string Int32 = typeof(int).ToString();
        public static string Int64 = typeof(Int64).ToString();
        public static string Double = typeof(double).ToString();
        public static string String = typeof(string).ToString();
        public static string Float = typeof(float).ToString();
        public static string Decimal = typeof(decimal).ToString();
        public static string DateTime = typeof(DateTime).ToString();
        public static string Byte = typeof(byte).ToString();
        public static string SByte = typeof(sbyte).ToString();
        public static string Char = typeof(char).ToString();
        public static string UInt = typeof(uint).ToString();
        public static string IntPtr = typeof(nint).ToString();
        public static string UIntPtr = typeof(nuint).ToString();
        public static string Long = typeof(long).ToString();
        public static string ULong = typeof(ulong).ToString();
        public static string Short = typeof(short).ToString();
        public static string UShort = typeof(ushort).ToString();
    }
    
    public static class XCollectionExtensions
    {
        /// <summary>
        /// 모델간 값 자동 바인딩 (UI용), 계층구조 지원 안함. 값 복사용.
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

        /*
         * xMapping 사용시 주의사항
         * 단일 테이블 객체에 대해서만 사용한다.
         * 복합 테이블 객체에 대해서는 Response용으로만 사용한다.
         * 복합 테이블 객체에 대해서는 맵핑을 사용하지 않는다. (Entity State가 유지 안됨)
         * 
         */
        
        /// <summary>
        /// <summary>
        /// 객체간 값 복사 (UI 갱신용 아님), 깊은 복사용
        /// </summary>
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T1 xMapping<T1>(this object src)
        {
            return src.Adapt<T1>();
        }

        /// <summary>
        /// 객체간 값 복사 (UI 갱신용 아님), 깊은 복사용, 다층 구조 지원안함.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="src">From</param>
        /// <param name="dest">To</param>
        /// <returns></returns>
        public static void xMapping<T1>(this T1 src, T1 dest)
        {
            var props = src.xGetProperties();
            props.xForEach(item =>
            {
                if (item.MemberType == MemberTypes.Property)
                {
                    if (item.PropertyType.ToString() == DataTypeName.Int16 ||
                        item.PropertyType.ToString() == DataTypeName.Int32 ||
                        item.PropertyType.ToString() == DataTypeName.Int64 ||
                        item.PropertyType.ToString() == DataTypeName.Double ||
                        item.PropertyType.ToString() == DataTypeName.String ||
                        item.PropertyType.ToString() == DataTypeName.Float ||
                        item.PropertyType.ToString() == DataTypeName.Decimal ||
                        item.PropertyType.ToString() == DataTypeName.DateTime ||
                        item.PropertyType.ToString() == DataTypeName.Byte ||
                        item.PropertyType.ToString() == DataTypeName.SByte ||
                        item.PropertyType.ToString() == DataTypeName.Char ||
                        item.PropertyType.ToString() == DataTypeName.UInt || 
                        item.PropertyType.ToString() == DataTypeName.IntPtr ||
                        item.PropertyType.ToString() == DataTypeName.UIntPtr ||
                        item.PropertyType.ToString() == DataTypeName.Long ||
                        item.PropertyType.ToString() == DataTypeName.ULong ||
                        item.PropertyType.ToString() == DataTypeName.Short ||
                        item.PropertyType.ToString() == DataTypeName.UShort)
                    {
                        if (item.Name.xContains(new[] { "CreatedOn", "CreatedBy", "CreatedName", "TenantId" })) return true;
                        var v = GetPropertyValue(src, item.Name);
                        SetPropertyValue(dest, item.Name, v);           
                    }
                }

                return true;
            });
        }

        /// <summary>
        /// 객체간 값 복사 (UI 갱신용 아님)
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="src"></param>
        /// <param name="mapping"></param>
        /// <param name="setter">Ignore 대상을 셋팅</param>
        /// <returns></returns>
        public static T2 xMapping<T1, T2>(this T1 src, Func<T1, T2, T2> mapping = null)
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
        {
            //deep copy
            var destMapped = dest.xMapping<T2>();
            
            //other deep copy
            var mapped = src.Adapt<T1, T2>(destMapped);
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
        /// <param name="value"></param>
        /// <returns></returns>
        public static IDictionary<string, object> xToDictionary<T>(this T value) where T : class
        {
            var result = new DynamicDictionary<object>();
            var props = value.GetType().GetProperties();
            foreach (var prop in props)
            {
                result.Add(prop.Name, prop.GetValue(value, null));
            }
            return result;
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