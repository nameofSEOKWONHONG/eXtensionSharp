using System.IO.Compression;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace eXtensionSharp
{
    public static class XStringExtensions
    {
        /// <summary>
        /// substring
        /// </summary>
        /// <param name="str"></param>
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
        public static string xSubstring(this string str, int startIndex, int length = 0)
        {
            if (str.xIsEmpty()) return string.Empty;
            if (str.Length <= 0) return string.Empty;
            if (length > 0) return str[startIndex..(startIndex + length)].ToString();
            return str[startIndex..str.Length].ToString();
        }

        /// <summary>
        /// substring use span
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static ReadOnlySpan<char> xSlice(this string str, int startIndex, int length = 0)
        {
            if(str.xIsEmpty()) return ReadOnlySpan<char>.Empty;
            if (str.Length <= 0) return ReadOnlySpan<char>.Empty;
            if(length <=0) return ReadOnlySpan<char>.Empty;
            
            return str.AsSpan(startIndex, length);
        }

        /// <summary>
        /// array or list string to join string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// var s = new string[]{"A", "B", "C"}
        /// var ss = s.xJoin(",");
        /// Console.WriteLine(ss); //output:"A,B,C";
        /// </code>
        /// </example>
        public static string xJoin<T>(this IEnumerable<T> src, string separator = ",")
        {
            if (src.xIsEmpty()) return string.Empty;
            return string.Join(separator, src);
        }

        /// <summary>
        /// get string length
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// var s = "abc";
        /// var ss = s.xCount();
        /// //output:3;
        /// or
        /// string s = null;
        /// var ss = s.xCount();
        /// //output:0;
        /// </code>
        /// </example>
        public static int xCount(this string str)
        {
            return str.xIsEmpty() ? 0 : str.Length;
        }

        public static int xCount(this string str, char word)
        {
            if (str.xIsEmpty()) return 0;
            return str.Count(x => x == word);
        }        

        /// <summary>
        /// get replace string
        /// </summary>
        /// <param name="text"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string xReplace(this string text, string oldValue, string newValue)
        {
            return text.xIsEmpty() ? string.Empty : text.Replace(oldValue, newValue);
        }

        /// <summary>
        /// normal text to hide text
        /// </summary>
        /// <param name="str"></param>
        /// <param name="replace"></param>
        /// <param name="startIdx"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// var s = "test@gmail.com";
        /// var ss = s.xHiddenText('*', 0, 4)
        /// Console.WriteLine(ss); //output:****@gmail.com 
        /// </code>
        /// </example>
        public static string xReplace(this string str, char replace, int startIdx, int length = 0)
        {
            if (str.xIsEmpty()) return string.Empty;

            var arr = str.ToArray();
            for (int i = startIdx; i <= startIdx + length; i++)
            {
                if (arr[i].xIsEmpty()) continue;
                arr[i] = replace;
            }

            return new System.String(arr);
        }

		/// <summary>
		/// gzip compression
		/// </summary>
		/// <param name="value"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		public static byte[] xCompress(this string value, CompressionLevel level = CompressionLevel.Fastest)
        {
            var bytes = Encoding.Unicode.GetBytes(value);

            using var memoryStream = new MemoryStream();
            using (var gzipStream = new GZipStream(memoryStream, level))
            {
                gzipStream.Write(bytes, 0, bytes.Length);
            }
            return memoryStream.GetBuffer();
        }

        /// <summary>
        /// gzip uncompression
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string xUnCompress(this byte[] bytes)
        {
            using var memoryStream = new MemoryStream(bytes);
            using var outputStream = new MemoryStream();
            using var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress);
            decompressStream.CopyTo(outputStream);

            var outputBytes = outputStream.ToArray();
            return Encoding.Unicode.GetString(outputBytes);
        }

		/// <summary>
		/// gzip compression
		/// </summary>
		/// <param name="value"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		public static async Task<byte[]> xCompressAsync(this string value, CompressionLevel level = CompressionLevel.Fastest)
		{
			var bytes = Encoding.Unicode.GetBytes(value);

            using var memoryStream = new MemoryStream();
            await using (var gzipStream = new GZipStream(memoryStream, level))
            {
                await gzipStream.WriteAsync(bytes, 0, bytes.Length);
            }
            return memoryStream.ToArray();
        }

		/// <summary>
		/// gzip uncompression
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static async Task<string> xUnCompressAsync(this byte[] bytes)
        {
            using var memoryStream = new MemoryStream(bytes);
            using var outputStream = new MemoryStream();
            await using var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress);
            await decompressStream.CopyToAsync(outputStream);

            var outputBytes = outputStream.ToArray();
            return Encoding.Unicode.GetString(outputBytes);
        }

		public static string xJoin(this string[] value, string separator)
        {
            return string.Join(separator, value);
        }

        public static int xIndexOf(this string src, string value)
        {
            if (value.xIsEmpty()) return -1;
            return src.IndexOf(value);
        }

        public static int xIndexOfAny(this string src, string value)
        {
            if (value.xIsEmpty()) return -1;
            return src.IndexOfAny(value.ToCharArray());
        }

        public static int xLastIndexOf(this string src, string value)
        {
            if (value.xIsEmpty()) return -1;
            return src.LastIndexOf(value);
        }

        public static int xLastIndexOfAny(this string src, string value)
        {
            if (value.xIsEmpty()) return -1;
            return src.LastIndexOfAny(value.ToCharArray());
        }

        public static string xTrim(this string src)
        {
            return src.xValue<string>(string.Empty).Trim();
        }

        public static string xGetHashCode(this string text)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
            var stringBuilder = new StringBuilder();
            foreach (var b in hash) stringBuilder.AppendFormat("{0:x2}", b);

            return stringBuilder.ToString();
        }

        public static byte[] xToBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string xToString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static string xDistinct(this string str)
        {
            var hash = new HashSet<char>();
            str.xForEach(item => { hash.Add(item); });

            return hash.xJoin(string.Empty);
        }

        public static string xDistinct(this IEnumerable<string> items)
        {
            var hash = new HashSet<string>();
            items.xForEach(item => { hash.Add(item); });

            return hash.xJoin();
        }

        public static string xToMySqlRLikeString(this IEnumerable<string> items)
        {
            return items.ToArray().xJoin("|");
        }

        public static Guid xToGuid(this string str) => Guid.Parse(str);

        public static string xToString(this Guid guid, string format = "") => guid.ToString();

        /// <summary>
        /// string to number
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="T">The type of the number to convert to.</typeparam>
        /// <returns>The converted number.</returns>
        /// <example> 
        /// <code>
        /// [example]
        /// var s = "5";
        /// var ss = s.xToNumber&lt;int&gt;();
        /// Console.WriteLine(ss); //output:5
        /// </code>
        /// </example>
        public static T xToNumber<T>(this string value) where T : struct
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// string to extract number
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// var s = "(5)";
        /// var ss = s.xExtractNumber&lt;int&gt;();
        /// Console.WriteLine(ss); //output:5
        /// </code>
        /// </example>
        public static T xExtractNumber<T>(this string value) where T : struct
        {
            // 숫자가 아닌 문자(\D)를 찾음
            string pattern = @"\D";
            // 숫자가 아닌 문자를 공백으로 대체
            string result = Regex.Replace(value, pattern, ""); 
            return result.xToNumber<T>();
        }

        /// <summary>
        /// seperate to string arry
        /// </summary>
        /// <param name="value"></param>
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
        public static string[] xSplit(this string value, string separator = ",")
        {
            if (value.xIsEmpty()) return Array.Empty<string>();
            return value.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }
        
        public static string xRandomString(this int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.

            // char is a single Unicode character
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26

            var random = new Random();
            for (var i = 0; i < size; i++)
            {
                var @char = (char)random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
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
        
        public static string xSubstringMiddle(this string value, int fromLen, int getLen)
        {
            if(value.xIsEmpty()) return string.Empty;
            return value.Substring(fromLen, getLen);
        }

        public static string xSubstringFirst(this string value, int length)
        {
            if(value.xIsEmpty()) return string.Empty;
            return value.Substring(0, length);
        }

        public static string xSubstringLast(this string value, int length)
        {
            if(value.xIsEmpty()) return string.Empty;
            return value.Substring(value.Length - length, length);
        }        
    }
}