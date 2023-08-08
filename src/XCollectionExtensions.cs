using System.Reflection;

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
            return propertyTypeName.xIsEquals(new string[]
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