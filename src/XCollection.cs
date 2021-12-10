using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eXtensionSharp
{
    public static class XCollection
    {
        #region [Datatable & DataReader]

        public static DataTable xToDateTable<T>(this IEnumerable<T> entities)
            where T : class, new()
        {
            var entity = new T();
            var properties = entity.GetType().GetProperties();

            var dt = new DataTable();
            foreach (var property in properties) dt.Columns.Add(property.Name, property.PropertyType);

            entities.xForEach(item =>
            {
                var itemProperty = item.GetType().GetProperties();
                var row = dt.NewRow();
                foreach (var property in itemProperty) row[property.Name] = property.GetValue(item);
                dt.Rows.Add(row);
                return true;
            });

            return dt;
        }

        public static T xReaderToObject<T>(this IDataReader reader)
            where T : class, new()
        {
            var properties = typeof(T).GetProperties().xToList();

            var newItem = new T();

            Enumerable.Range(0, reader.FieldCount - 1).xForEach(i =>
            {
                if (!reader.IsDBNull(i))
                {
                    var property = properties.Where(m => m.Name.Equals(reader.GetName(i))).xFirst();
                    if (!property.xIsEmpty())
                        if (reader.GetFieldType(i).Equals(property.PropertyType))
                            property.SetValue(newItem, reader[i]);
                }
            });

            return newItem;
        }

        #endregion [Datatable & DataReader]
    }

    public class ENUM_DATETIME_FOREACH_TYPE : XEnumBase<ENUM_DATETIME_FOREACH_TYPE>
    {
        public static readonly ENUM_DATETIME_FOREACH_TYPE DAY = Define("Day");
        public static readonly ENUM_DATETIME_FOREACH_TYPE MONTH = Define("Month");
        public static readonly ENUM_DATETIME_FOREACH_TYPE YEAR = Define("Year");
    }
}