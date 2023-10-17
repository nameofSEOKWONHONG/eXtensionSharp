using System.IO.Compression;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace eXtensionSharp
{
    public static class XStringExtensions
    {
        /*
         * Span변환의 장점 : 스택에 메모리 할당되므로 GC가 발생하지 않도록 해줌.
         * Memory<T> (T:Class) -> Span<T> (T:Struct)로 변환하여 사용 가능
         * new 대신 stackalloc 사용할 경우 GC 압력이 줄어듬.(struct type에 대하여, int, char, byte 등등)
         * windows stack 최대 할당 용량은 1MB
         */

        public static string xSubstring(this string str, int startIndex, int length = 0)
        {
            if (str.xIsEmpty()) return string.Empty;
            if (str.Length <= 0) return string.Empty;
            if (length > 0) return str.AsSpan()[startIndex..(startIndex + length)].ToString();
            return str.AsSpan()[startIndex..str.Length].ToString();
        }

        public static string xJoin<T>(this IEnumerable<T> src, string separator = ",")
        {
            return string.Join(separator, src);
        }

        public static int xCount(this string str)
        {
            return str.xIsNull() ? 0 : str.Length;
        }

        public static string xReplace(this string text, string oldValue, string newValue)
        {
            return text.xIsEmpty() ? string.Empty : text.Replace(oldValue, newValue);
        }

        private static void xCopyTo(Stream src, Stream dest)
        {
            var bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) dest.Write(bytes, 0, cnt);
        }

        /// <summary>
        /// compression gzip
        /// </summary>
        /// <param name="str"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static byte[] xCompressGZip(this string str, CompressionLevel level = CompressionLevel.Fastest)
        {
            var bytes = Encoding.Unicode.GetBytes(str);
            using var input = new MemoryStream(bytes);
            using var output = new MemoryStream();
            using var stream = new GZipStream(output, level);

            input.CopyTo(stream);
            output.Flush();

            var result = output.ToArray();
            return result;
        }

        /// <summary>
        /// uncompression gzip
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string xUnCompressGZip(this string value)
        {
            var bytes = Convert.FromBase64String(value);
            using var input = new MemoryStream(bytes);
            using var output = new MemoryStream();
            using var stream = new GZipStream(input, CompressionMode.Decompress);

            stream.CopyTo(output);
            stream.Flush();

            return Encoding.Unicode.GetString(output.ToArray());
        }

        /// <summary>
        /// compression brotli
        /// </summary>
        /// <param name="str"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static byte[] xCompressBrotli(this string str, CompressionLevel level = CompressionLevel.Fastest)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            {
                using (var mso = new MemoryStream())
                {
                    using (var stream = new BrotliStream(mso, level))
                    {
                        msi.CopyTo(stream);
                        mso.Flush();

                        var result = mso.ToArray();
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// uncompression brotli
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string xUnCompressBrotli(this string value)
        {
            var bytes = Convert.FromBase64String(value);
            using var input = new MemoryStream(bytes);
            using var output = new MemoryStream();
            using var stream = new BrotliStream(input, CompressionMode.Decompress);

            stream.CopyTo(output);

            return Encoding.Unicode.GetString(output.ToArray());
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
            return src.xIsEmpty() ? string.Empty : src.Trim();
        }

        public static string xGetHashCode(this string text)
        {
            var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
            var stringBuilder = new StringBuilder();
            foreach (var b in hash) stringBuilder.AppendFormat("{0:x2}", b);

            return stringBuilder.ToString();
        }

        public static byte[] xToBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static byte[] xToBytes<T>(this T value)
        {
            var objToString = System.Text.Json.JsonSerializer.Serialize(value);
            return System.Text.Encoding.UTF8.GetBytes(objToString);
        }

        public static string xToString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static int xCount(this string str, char word)
        {
            if (str.xIsEmpty()) return 0;
            return str.Count(x => x == word);
        }

        public static string xDistinct(this string str)
        {
            var hash = new HashSet<char>();
            str.xForEach(item =>
            {
                if (!hash.Contains(item))
                {
                    hash.Add(item);
                }
            });

            return string.Join("", hash);
        }

        public static string xToMySqlRLikeString(this IEnumerable<string> items)
        {
            return string.Join('|', items.ToArray());
        }

        public static Guid xToGuid(this string str) => Guid.Parse(str);

        public static string xToString(this Guid guid, string format = "") => guid.ToString();

        public static T xToNumber<T>(this string value) where T : struct
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static string xToJoin(this string[] values, string separator = ",")
        {
            if (values.xIsEmpty()) return string.Empty;
            return string.Join(separator, values);
        }

        /// <summary>
        /// 문자열 분리
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator">'§'</param>
        /// <returns></returns>
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
        
        private static string xToString(Expression expr)
        {
            switch (expr.NodeType)
            {
                case ExpressionType.Lambda:
                    //x => (Something), return only (Something), the Body
                    return xToString(((LambdaExpression)expr).Body);

                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    //type casts are not important
                    return xToString(((UnaryExpression)expr).Operand);

                case ExpressionType.Call:
                    //method call can be an Indexer (get_Item),
                    var callExpr = (MethodCallExpression)expr;
                    if (callExpr.Method.Name == "get_Item")
                    {
                        //indexer call
                        return xToString(callExpr.Object) + "[" +
                               string.Join(",", callExpr.Arguments.Select(xToString)) + "]";
                    }
                    else
                    {
                        //method call
                        var arguments = callExpr.Arguments.Select(xToString).ToArray();
                        string target;
                        if (callExpr.Method.IsDefined(typeof(ExtensionAttribute), false))
                        {
                            //extension method
                            target = string.Join(".", arguments[0], callExpr.Method.Name);
                            arguments = arguments.Skip(1).ToArray();
                        }
                        else if (callExpr.Object == null)
                        {
                            //static method
                            target = callExpr.Method.Name;
                        }
                        else
                        {
                            //instance method
                            target = string.Join(".", xToString(callExpr.Object), callExpr.Method.Name);
                        }

                        return target + "(" + string.Join(",", arguments) + ")";
                    }
                case ExpressionType.MemberAccess:
                    //property or field access
                    var memberExpr = (MemberExpression)expr;
                    if (memberExpr.Expression.Type.Name.Contains("<>")) //closure type, don't show it.
                        return memberExpr.Member.Name;
                    else
                        return string.Join(".", xToString(memberExpr.Expression), memberExpr.Member.Name);
            }

            //by default, show the standard implementation
            return expr.ToString();
        }
        
        public static string xHiddenText(this string str, char hiddenChar, int startIdx, int length = 0)
        {
            if (str.xIsEmpty()) return string.Empty;
            
            var arr = str.ToArray();
        
            for (int i = startIdx; i <= startIdx + length; i++)
            {
                if(arr[i].xIsEmpty()) continue;
                arr[i] = hiddenChar;
            }
            
            return new String(arr);
        }
    }
}