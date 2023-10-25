using System.Collections.Immutable;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace eXtensionSharp
{
    public static class XCollectionExtensions
    {
        public static void xValueChange<T1>(this T1 src, T1 dest)
        {
            var props = src.xGetProperties();
            props.xForEach(item =>
            {
                if (item.CanWrite.xIsFalse()) return true;
                var v = GetPropertyValue(src, item.Name);
                SetPropertyValue(dest, item.Name, v);

                return true;
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
        
        public static void xMapping<T1>(this T1 src, T1 dest)
        {
            var props = src.xGetProperties();
            props.xForEach(item =>
            {
                if (item.MemberType == MemberTypes.Property)
                {
                    if (IsTypeMatch(item.PropertyType.ToString()))
                    {
                        if (item.Name.xContains(new[] { "CreatedOn", "CreatedBy", "CreatedName", "TenantId" })) return true;
                        var v = GetPropertyValue(src, item.Name);
                        SetPropertyValue(dest, item.Name, v);           
                    }
                }
                return true;
            });
        }

        public static void xMapping<T1, T2>(this T1 src, T2 dest)
        {
            var props = src.xGetProperties();
            props.xForEach(item =>
            {
                if (item.MemberType == MemberTypes.Property)
                {
                    if (IsTypeMatch(item.PropertyType.ToString()))
                    {
                        if (item.Name.xContains(new[] { "CreatedOn", "CreatedBy", "CreatedName", "TenantId" })) return true;
                        var v = GetPropertyValue(src, item.Name);
                        var exist = dest.xGetProperties().Where(m => m.Name == item.Name);
                        if (exist.Any())
                        {
                            SetPropertyValue(dest, item.Name, v);    
                        }       
                    }
                }
                return true;
            });
        } 

        private static bool IsTypeMatch(string propertyTypeName)
        {
            return propertyTypeName.xContains(new string[]
            {
                DataTypeName.Int16,
                DataTypeName.Int32,
                DataTypeName.Int64,
                DataTypeName.Double,
                DataTypeName.String,
                DataTypeName.Float,
                DataTypeName.Decimal,
                DataTypeName.DateTime,
                DataTypeName.Byte,
                DataTypeName.SByte,
                DataTypeName.Char,
                DataTypeName.UInt, 
                DataTypeName.IntPtr,
                DataTypeName.UIntPtr,
                DataTypeName.Long,
                DataTypeName.ULong,
                DataTypeName.Short,
                DataTypeName.UShort,
                
                DataTypeName.NullableInt16,
                DataTypeName.NullableInt32,
                DataTypeName.NullableInt64,
                DataTypeName.NullableDouble,
                DataTypeName.NullableFloat,
                DataTypeName.NullableDecimal,
                DataTypeName.NullableDateTime,
                DataTypeName.NullableByte,
                DataTypeName.NullableSByte,
                DataTypeName.NullableChar,
                DataTypeName.NullableUInt, 
                DataTypeName.NullableIntPtr,
                DataTypeName.NullableUIntPtr,
                DataTypeName.NullableLong,
                DataTypeName.NullableULong,
                DataTypeName.NullableShort,
                DataTypeName.NullableUShort,
                DataTypeName.NullableDateTime
            });
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
        
        public static Dictionary<string, T2> xToDictionary<T, T2>(this T value) where T : class
        {
            var result = new Dictionary<string, T2>();
            var props = value.GetType().GetProperties();
            foreach (var prop in props)
            {
                result.Add(prop.Name, prop.GetValue(value, null).xValue<T2>());
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
        
        public static int xCount<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null)
        {
            if (enumerable.xIsEmpty()) return 0;
            if (predicate.xIsNotEmpty()) return enumerable.Count(predicate);
            return enumerable.Count();
        }

        public static IEnumerable<T> xWhere<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
            where T : class
        {
            return CreateOrEnumerable(enumerable).Where(predicate);
        }

        public static IEnumerable<T2> xSelect<T1, T2>(this IEnumerable<T1> enumerable, Func<T1, T2> predicate)
            where T1 : class
        {
            return CreateOrEnumerable(enumerable).Select(predicate);
        }

        private static IEnumerable<T> CreateOrEnumerable<T>(IEnumerable<T> enumerable)
        {
            if (enumerable.xIsEmpty()) return Enumerable.Empty<T>();
            return enumerable;
        }

        public static bool xContains(this string src, string compare)
        {
            if (src.xIsEmpty()) return false;
            return src.Contains(compare);
        }

        public static bool xContains(this string src, string[] compares)
        {
            if (src.xIsEmpty()) return false;
            return compares.FirstOrDefault(src.Contains).xIsNotEmpty();
        }

        public static bool xContains<T>(this T src, IEnumerable<T> compares)
        {
            if (src.xIsEmpty()) return false;
            return compares.Contains(src);
        }

        public static bool xContains<T>(this IEnumerable<T> src, IEnumerable<T> compares)
        {
            if (src.xIsEmpty()) return false;
            return src.Where(m => m.xContains(compares)).xIsNotEmpty();
        }        
        
        public static T xFirst<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null)
        {
            if (predicate.xIsNotEmpty()) return enumerable.FirstOrDefault(predicate);
            return enumerable.FirstOrDefault();
        }

        public static T xLast<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null)
        {
            if (predicate.xIsNotEmpty()) return enumerable.LastOrDefault(predicate);
            return enumerable.LastOrDefault();
        }

        public static List<T> xToList<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.xIsEmpty() ? new List<T>() : new List<T>(enumerable);
        }

        public static T[] xToArray<T>(this IEnumerable<T> enumerable) where T : new()
        {
            if (enumerable.xIsEmpty()) return new T[0];
            return enumerable.ToArray();
        }

        #region [between]

        #region [.net 7에서 inumber를 사용하여 하나의 function으로 변경됨.]

        public static bool xIsBetween<T>(this T value, T from, T to)
            where T : INumber<T>
        {
            if (from > to) throw new Exception("from value is greater than to value.");
            if (from <= value && to >= value) return true;
            return false;
        }

        #endregion [.net 7에서 inumber를 사용하여 하나의 function으로 변경됨.]

        public static bool xIsBetween(this DateTime value, DateTime from, DateTime to)
        {
            if (from <= value && to >= value) return true;
            return false;
        }

        public static bool xIsBetween(this TimeSpan value, TimeSpan from, TimeSpan to)
        {
            if (value <= TimeSpan.Zero) throw new Exception("not allow value");
            if (from <= value && to >= value) return true;
            return false;
        }
        
        public static bool xIsBetween(this TimeSpan value, TimeSpan? from, TimeSpan? to)
        {
            if (from.xIsEmpty()) throw new Exception("from is empty");
            if (to.xIsEmpty()) throw new Exception("to is empty");
            
            if (value <= TimeSpan.Zero) throw new Exception("not allow value");
            
            if (from <= value && to >= value) return true;
            return false;
        }        

        public static bool xIsBetween(this char value, char from, char to, bool isStrict = true)
        {
            var v = Convert.ToByte(isStrict ? value : char.ToUpper(value));
            var f = Convert.ToByte(isStrict ? from : char.ToUpper(from));
            var t = Convert.ToByte(isStrict ? to : char.ToUpper(to));
            if (f <= v && t >= v) return true;
            return false;
        }

        #endregion [between]
        
        public static Span<T> xToSpan<T>(this IEnumerable<T> items) where T : class, new()
        {
            return items.xToArray().AsSpan();
        }

        public static ImmutableArray<T> xToImmutableArray<T>(this IEnumerable<T> items)
        {
            var array = items.ToArray();
            return Unsafe.As<T[], ImmutableArray<T>>(ref array);
        }

        public static T xLikeFirst<T>(this IEnumerable<T> item1, IEnumerable<T> item2)
        {
            return item1.FirstOrDefault(item2.Contains);
        }

        public static IEnumerable<T> xLike<T>(this IEnumerable<T> item1, IEnumerable<T> item2)
        {
            return item1.Where(item2.Contains);
        }
    }
}