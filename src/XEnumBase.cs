using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace eXtensionSharp
{
    // ref : https://github.com/gerardog/StringEnum/blob/master/StringEnum/StringEnum.cs
    // For Newtonsoft.Json support check /StringEnum.Sample.NewtonsoftSerialization/StringEnum.cs .</remarks>
    /// <summary>
    ///     Base class for creating string-valued enums in .NET.<br />
    ///     Provides static Parse() and TryParse() methods and implicit cast to string.
    /// </summary>
    /// <example>
    ///     <code>
    /// class Color : StringEnum &lt;Color&gt;
    /// {
    ///     public static readonly Color Blue = Create("Blue");
    ///     public static readonly Color Red = Create("Red");
    ///     public static readonly Color Green = Create("Green");
    /// }
    /// </code>
    /// </example>
    /// <typeparam name="T">The string-valued enum type. (i.e. class Color : StringEnum&lt;Color&gt;)</typeparam>
    public abstract class XEnumBase<T> : IEquatable<T> where T : XEnumBase<T>, new()
    {
        private static readonly Dictionary<string, T> valueDict = new();
        protected string Value;

        bool IEquatable<T>.Equals(T other)
        {
            return Value.Equals(other.Value);
        }

        protected static T Define(string value)
        {
            if (value == null)
                return null; // the null-valued instance is null.

            var result = new T {Value = value};
            valueDict.Add(value, result);
            return result;
        }

        public static implicit operator string(XEnumBase<T> enumValue)
        {
            return enumValue.Value;
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator !=(XEnumBase<T> o1, XEnumBase<T> o2)
        {
            return o1?.Value != o2?.Value;
        }

        public static bool operator ==(XEnumBase<T> o1, XEnumBase<T> o2)
        {
            return o1?.Value == o2?.Value;
        }

        public override bool Equals(object other)
        {
            return Value.Equals((other as T)?.Value ?? other as string);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        ///     Parse the <paramref name="value" /> specified and returns a valid <typeparamref name="T" /> or else throws
        ///     InvalidOperationException.
        /// </summary>
        /// <param name="value">
        ///     The string value representad by an instance of <typeparamref name="T" />. Matches by string value,
        ///     not by the member name.
        /// </param>
        /// <param name="caseSensitive">
        ///     If true, the strings must match case and takes O(log n). False allows different case but is
        ///     little bit slower (O(n))
        /// </param>
        public static T Parse(string value, bool caseSensitive = true)
        {
            var result = TryParse(value, caseSensitive);
            if (result == null)
                throw new InvalidOperationException((value == null ? "null" : $"'{value}'") +
                                                    $" is not a valid {typeof(T).Name}");

            return result;
        }

        /// <summary>
        ///     Parse the <paramref name="value" /> specified and returns a valid <typeparamref name="T" /> or else returns null.
        /// </summary>
        /// <param name="value">
        ///     The string value representad by an instance of <typeparamref name="T" />. Matches by string value,
        ///     not by the member name.
        /// </param>
        /// <param name="caseSensitive">If true, the strings must match case. False allows different case but is slower: O(n)</param>
        public static T TryParse(string value, bool caseSensitive = true)
        {
            if (value == null) return null;
            if (valueDict.Count == 0)
                RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle); // force static fields initialization
            if (caseSensitive)
            {
                if (valueDict.TryGetValue(value, out var item))
                    return item;
                return null;
            }

            // slower O(n) case insensitive search
            return valueDict.FirstOrDefault(f => f.Key.Equals(value, StringComparison.OrdinalIgnoreCase)).Value;
            // Why Ordinal? => https://esmithy.net/2007/10/15/why-stringcomparisonordinal-is-usually-the-right-choice/
        }
    }
}