using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace eXtensionSharp.V2;

public static class StringExtensions
{
    extension(string source)
    {
        /// <summary>
        /// substring
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// var s = "test";
        /// var ss = s.xSubstring(0, 1);
        /// Console.WriteLine(ss); //output:"t"
        ///
        /// var s = "";
        /// var ss = s.xSubstring(0,1);
        /// Console.WriteLine(ss); //output:""
        /// </code>
        /// </example>
        public string xSubstring(int startIdx, int length = 0)
        {
            if (source.xIsEmpty()) return string.Empty;
            if (source.Length <= 0) return string.Empty;
            if (length > 0) return source[startIdx..(startIdx + length)].ToString();
            return source[startIdx..source.Length].ToString();
        }

        /// <summary>
        /// substring use span
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public ReadOnlySpan<char> xSlice(int startIndex, int length = 0)
        {
            if(source.xIsEmpty()) return ReadOnlySpan<char>.Empty;
            if (source.Length <= 0) return ReadOnlySpan<char>.Empty;
            if(length <=0) return ReadOnlySpan<char>.Empty;
            
            return source.AsSpan(startIndex, length);
        }

        /// <summary>
        /// get string length
        /// </summary>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// var s = "abc";
        /// var ss = s.xLength();
        /// //output:3;
        /// or
        /// string s = null;
        /// var ss = s.xLength();
        /// //output:0;
        /// </code>
        /// </example>
        public int xLength()
        {
            return source.xIsEmpty() ? 0 : source.Length;
        }

        /// <summary>
        /// get replace string
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public string xReplace(string oldValue, string newValue)
        {
            return source.xIsEmpty() ? string.Empty : source.Replace(oldValue, newValue);
        }

        /// <summary>
        /// gzip compression
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public byte[] xCompress(CompressionLevel level = CompressionLevel.Fastest)
        {
            var bytes = Encoding.Unicode.GetBytes(source);

            using var memoryStream = new MemoryStream();
            using (var gzipStream = new GZipStream(memoryStream, level))
            {
                gzipStream.Write(bytes, 0, bytes.Length);
            }
            return memoryStream.GetBuffer();
        }

        /// <summary>
        /// gzip compression
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public async Task<byte[]> xCompressAsync(CompressionLevel level = CompressionLevel.Fastest)
        {
            var bytes = Encoding.Unicode.GetBytes(source);

            using var memoryStream = new MemoryStream();
            await using (var gzipStream = new GZipStream(memoryStream, level))
            {
                await gzipStream.WriteAsync(bytes, 0, bytes.Length);
            }
            return memoryStream.ToArray();
        }

        public string xJoin(string separator)
        {
            return string.Join(separator, source);
        }

        public int xIndexOf(string value)
        {
            if (value.xIsEmpty()) return -1;
            return source.IndexOf(value);
        }

        public int xIndexOfAny(string value)
        {
            if (value.xIsEmpty()) return -1;
            return source.IndexOfAny(value.ToCharArray());
        }

        public int xLastIndexOf( string value)
        {
            if (value.xIsEmpty()) return -1;
            return source.LastIndexOf(value);
        }

        public int xLastIndexOfAny(string value)
        {
            if (value.xIsEmpty()) return -1;
            return source.LastIndexOfAny(value.ToCharArray());
        }

        public string xTrim()
        {
            return source.xValue<string>(string.Empty).Trim();
        }

        public string xGetHashCode()
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(source));
            var stringBuilder = new StringBuilder();
            foreach (var b in hash) stringBuilder.AppendFormat("{0:x2}", b);

            return stringBuilder.ToString();
        }

        public byte[] xToUtf8Bytes()
        {
            return Encoding.UTF8.GetBytes(source);
        }



        public string xDistinct()
        {
            var hash = new HashSet<char>();
            source.xForEach(item => { hash.Add(item); });

            return hash.xJoin(string.Empty);
        }

        public Guid xToGuid()
        {
            if(Guid.TryParse(source, out var id))
            {
                return id;
            }

            return Guid.Empty;
        }

        /// <summary>
        /// string to extract number
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// var s = "(5)";
        /// var ss = s.xExtractNumber&lt;int&gt;();
        /// Console.WriteLine(ss); //output:5
        /// </code>
        /// </example>
        public T xExtractNumber<T>() where T : struct
        {
            // 숫자가 아닌 문자(\D)를 찾음
            string pattern = @"\D";
            // 숫자가 아닌 문자를 공백으로 대체
            string result = Regex.Replace(source, pattern, ""); 
            return result.xValue<T>();
        }

        /// <summary>
        /// seperate to string arry
        /// </summary>
        /// <param name="separator">'§'</param>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// var s = "A,B,C";
        /// var ss = s.xSplit(",");
        /// foreach(var item in ss) {
        ///     Console.WriteLine(item);
        /// }
        /// </code>
        /// </example>
        public string[] xSplit(string separator = ",")
        {
            if (source.xIsEmpty()) return Array.Empty<string>();
            return source.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }
        

        
        // private static string xToString(Expression expr)
        // {
        //     switch (expr.NodeType)
        //     {
        //         case ExpressionType.Lambda:
        //             //x => (Something), return only (Something), the Body
        //             return xToString(((LambdaExpression)expr).Body);
        //
        //         case ExpressionType.Convert:
        //         case ExpressionType.ConvertChecked:
        //             //type casts are not important
        //             return xToString(((UnaryExpression)expr).Operand);
        //
        //         case ExpressionType.Call:
        //             //method call can be an Indexer (get_Item),
        //             var callExpr = (MethodCallExpression)expr;
        //             if (callExpr.Method.Name == "get_Item")
        //             {
        //                 //indexer call
        //                 return xToString(callExpr.Object) + "[" +
        //                        string.Join(",", callExpr.Arguments.Select(xToString)) + "]";
        //             }
        //             else
        //             {
        //                 //method call
        //                 var arguments = callExpr.Arguments.Select(xToString).ToArray();
        //                 string target;
        //                 if (callExpr.Method.IsDefined(typeof(ExtensionAttribute), false))
        //                 {
        //                     //extension method
        //                     target = string.Join(".", arguments[0], callExpr.Method.Name);
        //                     arguments = arguments.Skip(1).ToArray();
        //                 }
        //                 else if (callExpr.Object == null)
        //                 {
        //                     //static method
        //                     target = callExpr.Method.Name;
        //                 }
        //                 else
        //                 {
        //                     //instance method
        //                     target = string.Join(".", xToString(callExpr.Object), callExpr.Method.Name);
        //                 }
        //
        //                 return target + "(" + string.Join(",", arguments) + ")";
        //             }
        //         case ExpressionType.MemberAccess:
        //             //property or field access
        //             var memberExpr = (MemberExpression)expr;
        //             if (memberExpr.Expression.Type.Name.Contains("<>")) //closure type, don't show it.
        //                 return memberExpr.Member.Name;
        //             else
        //                 return string.Join(".", xToString(memberExpr.Expression), memberExpr.Member.Name);
        //     }
        //
        //     //by default, show the standard implementation
        //     return expr.ToString();
        // }
        
        public string xSubstringMiddle(int fromLen, int getLen)
        {
            if(source.xIsEmpty()) return string.Empty;
            return source.Substring(fromLen, getLen);
        }

        public string xSubstringFirst(int length)
        {
            if(source.xIsEmpty()) return string.Empty;
            return source.Substring(0, length);
        }

        public string xSubstringLast(int length)
        {
            if(source.xIsEmpty()) return string.Empty;
            return source.Substring(source.Length - length, length);
        }

        /// <summary>
        /// Converts a string value to a specified type based on the provided type name.
        /// </summary>
        /// <param name="src">The source string value to be converted.</param>
        /// <param name="typeName">The name of the type to which the source value should be converted (e.g., "String", "Int32", "Boolean").</param>
        /// <returns>An object representing the converted value of the specified type.</returns>
        public object xValueByTypeName(string typeName)
        {
            return typeName switch
            {
                nameof(String) => source.xValue<string>(),
                nameof(Int32) => source.xValue<int>(),
                nameof(Int64) => source.xValue<long>(),
                nameof(Double) => source.xValue<double>(),
                nameof(Decimal) => source.xValue<decimal>(),
                nameof(Boolean) => source.xValue<bool>(),
                nameof(DateTime) => source.xValue<DateTime>(),
                nameof(DateTimeOffset) => source.xValue<DateTimeOffset>(),
                nameof(TimeSpan) => source.xValue<TimeSpan>(),
                nameof(Guid) => source.xValue<Guid>(),
                nameof(Byte) => source.xValue<byte>(),
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Compares two strings for equality, ignoring case sensitivity.
        /// </summary>
        /// <param name="dest">The destination string to compare against.</param>
        /// <returns>True if the strings are equal ignoring case; otherwise, false.</returns>
        /// <example>
        /// <code>
        /// string name1 = "hello";
        /// string name2 = "HELLO";
        /// bool areEqual = string.Equals(name1, name2, StringComparison.OrdinalIgnoreCase); // returns true
        /// </code>
        /// </example>
        public bool xEquals(string dest)
        {
            return string.Equals(source, dest, StringComparison.OrdinalIgnoreCase);
        }

        public T xDeserialize<T>(JsonSerializerOptions options = null)
        {
            if (options.xIsEmpty())
            {
                options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true, 
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                };
            }
            return JsonSerializer.Deserialize<T>(source, options);
        }

        public bool xContains(IEnumerable<string> arr)
        {
            return arr.Contains(source);
        } 

        /// <summary>
        /// Checks if a property type matches one of the predefined types (e.g., int, string, DateTime, etc.).
        /// </summary>
        /// <param name="propertyTypeName">The name of the property type to check.</param>
        /// <returns>True if the property type matches one of the predefined types, otherwise false.</returns>
        public bool xIsTypeMatch()
        {
            return source.xContains(new string[]
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

        public string xCultureDiaplayName()
        {
            if (source.xIsEmpty()) return string.Empty;
            
            var culture = new System.Globalization.CultureInfo(source);
            return culture.DisplayName;
        }

        public string xCultureEnglishName()
        {
            if (source.xIsEmpty()) return string.Empty;
            
            var culture = new System.Globalization.CultureInfo(source);
            return culture.EnglishName;
        }    

        /// <summary>
        /// Converts a string representation of a date to a <see cref="DateTime"/> object.
        /// </summary>
        /// <returns>A <see cref="DateTime"/> object if successful; otherwise, null.</returns>
        public DateTime? xConvertToDate()
        {
            if (DateTime.TryParse(source, out var datetime))
                return datetime;

            return null;
        }            
    }
}