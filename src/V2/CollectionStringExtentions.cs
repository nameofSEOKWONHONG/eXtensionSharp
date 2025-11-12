using System;

namespace eXtensionSharp.V2;

public static class CollectionStringExtentions
{
    extension(IEnumerable<string> source)
    {
        public string xToMySqlRLikeString()
        {
            if(source.xIsEmpty()) return string.Empty;
            return source.xJoin("|");
        }

        public string xJoin(string seperate = ",")
        {
            if(source.xIsEmpty()) return string.Empty;
            return string.Join(seperate, source);
        }
    }
}

public static class GenericExteions
{
    extension<T>(T source) where T : class
    {
        /// <summary>
        /// Gets the value of a specified property of an object.
        /// </summary>
        /// <param name="obj">The object whose property value is to be retrieved.</param>
        /// <param name="propertyName">The name of the property to get the value of.</param>
        /// <returns>The value of the property, or null if the property does not exist.</returns>
        public T2 GetPropertyValue<T2>(string propertyName)
        {
            var propertyInfo = source.GetType().GetProperty(propertyName);
            var v = propertyInfo?.GetValue(source);
            return v.xValue<T2>();
        }


        /// <summary>
        /// Sets the value of a specified property of an object.
        /// </summary>
        /// <param name="obj">The object whose property value is to be set.</param>
        /// <param name="propertyName">The name of the property to set the value of.</param>
        /// <param name="value">The value to set the property to.</param>
        public void SetPropertyValue(string propertyName, object value)
        {
            var propertyInfo = source.GetType().GetProperty(propertyName);
            propertyInfo?.SetValue(source, value);
        }
    }
}