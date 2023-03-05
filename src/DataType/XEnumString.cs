using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace eXtensionSharp
{
    /// <summary>
    ///     enum string class, use attribute
    /// </summary>
    public class XEnumString
    {
        #region Instance implementation

        private static readonly Hashtable _stringValues = new();

        /// <summary>
        ///     Creates a new <see cref="StringEnum" /> instance.
        /// </summary>
        /// <param name="enumType">Enum type.</param>
        public XEnumString(Type enumType)
        {
            if (!typeof(Type).GetTypeInfo().IsEnum)
                throw new ArgumentException($"Supplied type must be an Enum.  Type was {enumType}");

            EnumType = enumType;
        }

        /// <summary>
        ///     Gets the string value associated with the given enum value.
        /// </summary>
        /// <param name="valueName">Name of the enum value.</param>
        /// <returns>String Value</returns>
        public string GetStringValue(string valueName)
        {
            string stringValue = null;
            try
            {
                var enumType = (Enum)Enum.Parse(EnumType, valueName);
                stringValue = GetStringValue(enumType);
            }
            catch (Exception)
            {
                // ignored
            } //Swallow!

            return stringValue;
        }

        /// <summary>
        ///     Gets the string values associated with the enum.
        /// </summary>
        /// <returns>String value array</returns>
        public Array GetStringValues()
        {
            var values = new ArrayList();
            //Look for our string value associated with fields in this enum
            foreach (var fi in EnumType.GetFields()) //Check for our custom attribute
                if (fi.GetCustomAttributes(typeof(XEnumStringValueAttribute), false) is XEnumStringValueAttribute[]
                        attrs &&
                    attrs.Length > 0)
                    values.Add(attrs[0].Value);

            return values.ToArray();
        }

        /// <summary>
        ///     Gets the values as a 'bindable' list datasource.
        /// </summary>
        /// <returns>IList for data binding</returns>
        public IList GetListValues()
        {
            var underlyingType = Enum.GetUnderlyingType(EnumType);
            var values = new ArrayList();
            //Look for our string value associated with fields in this enum
            foreach (var fi in EnumType.GetFields()) //Check for our custom attribute
                if (fi.GetCustomAttributes(typeof(XEnumStringValueAttribute), false) is XEnumStringValueAttribute[]
                        attrs &&
                    attrs.Length > 0)
                    values.Add(new DictionaryEntry(Convert.ChangeType(Enum.Parse(EnumType, fi.Name), underlyingType),
                        attrs[0].Value));

            return values;
        }

        /// <summary>
        ///     Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <returns>Existence of the string value</returns>
        public bool IsStringDefined(string stringValue)
        {
            return Parse(EnumType, stringValue) != null;
        }

        /// <summary>
        ///     Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Existence of the string value</returns>
        public bool IsStringDefined(string stringValue, bool ignoreCase)
        {
            return Parse(EnumType, stringValue, ignoreCase) != null;
        }

        /// <summary>
        ///     Gets the underlying enum type for this instance.
        /// </summary>
        /// <value></value>
        private Type EnumType { get; }

        #endregion Instance implementation

        #region Static implementation

        /// <summary>
        ///     Gets a string value for a particular enum value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>String Value associated via a <see cref="XEnumStringValueAttribute" /> attribute, or null if not found.</returns>
        public static string GetStringValue(Enum value)
        {
            string output = null;
            var type = value.GetType();

            if (_stringValues.ContainsKey(value))
            {
                output = (_stringValues[value] as XEnumStringValueAttribute)?.Value;
            }
            else
            {
                //Look for our 'XEnumStringValueAttribute' in the field's custom attributes
                var fi = type.GetField(value.ToString());
                if (fi?.GetCustomAttributes(typeof(XEnumStringValueAttribute), false) is XEnumStringValueAttribute[]
                        attrs &&
                    attrs.Length > 0)
                {
                    _stringValues.Add(value, attrs[0]);
                    output = attrs[0].Value;
                }
            }

            return output;
        }

        /// <summary>
        ///     Parses the supplied enum and string value to find an associated enum value (case sensitive).
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="stringValue">String value.</param>
        /// <returns>Enum value associated with the string value, or null if not found.</returns>
        public static object Parse(Type type, string stringValue)
        {
            return Parse(type, stringValue, false);
        }

        /// <summary>
        ///     Parses the supplied enum and string value to find an associated enum value.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="stringValue">String value.</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Enum value associated with the string value, or null if not found.</returns>
        public static object Parse(Type type, string stringValue, bool ignoreCase)
        {
            object output = null;
            string enumStringValue = null;

            if (!typeof(Type).GetTypeInfo().IsEnum)
                throw new ArgumentException(string.Format("Supplied type must be an Enum.  Type was {0}", type));

            //Look for our string value associated with fields in this enum
            foreach (var fi in type.GetFields())
            {
                //Check for our custom attribute
                if (fi.GetCustomAttributes(typeof(XEnumStringValueAttribute), false) is XEnumStringValueAttribute[]
                        attrs &&
                    attrs.Length > 0)
                    enumStringValue = attrs[0].Value;

                //Check for equality then select actual enum value.
                if (string.Compare(enumStringValue, stringValue, ignoreCase) == 0)
                {
                    output = Enum.Parse(type, fi.Name);
                    break;
                }
            }

            return output;
        }

        /// <summary>
        ///     Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <param name="enumType">Type of enum</param>
        /// <returns>Existence of the string value</returns>
        public static bool IsStringDefined(Type enumType, string stringValue)
        {
            return Parse(enumType, stringValue) != null;
        }

        /// <summary>
        ///     Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <param name="enumType">Type of enum</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Existence of the string value</returns>
        public static bool IsStringDefined(Type enumType, string stringValue, bool ignoreCase)
        {
            return Parse(enumType, stringValue, ignoreCase) != null;
        }

        #endregion Static implementation
    }

    #region Class XEnumStringValueAttribute

    /// <summary>
    ///     Simple attribute class for storing String Values
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class XEnumStringValueAttribute : Attribute
    {
        /// <summary>
        ///     Creates a new <see cref="XEnumStringValueAttribute" /> instance.
        /// </summary>
        /// <param name="value">Value.</param>
        public XEnumStringValueAttribute(string value)
        {
            Value = value;
        }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <value></value>
        public string Value { get; }
    }

    #endregion Class XEnumStringValueAttribute

    #region [static enum util]

    public static class XEnumStringUtil
    {
        public static T xStringToEnum<T>(this string value) where T : struct
        {
            return Enum.TryParse(value, true, out T result) ? result : default;
        }

        public static string xEnumToString(this Enum value)
        {
            var da = (DescriptionAttribute[])value.GetType().GetField(value.ToString())
                ?.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return da != null && da.Length > 0 ? da[0].Description : value.ToString();
        }
    }

    #endregion [static enum util]
}