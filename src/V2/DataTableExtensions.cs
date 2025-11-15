using System;
using System.Data;

namespace eXtensionSharp.V2;

public static class DataTableExtensions
{
    extension(DataTable source)
    {
        /// <summary>
        /// Converts a DataTable to a collection of dictionaries where each dictionary represents a row in the DataTable.
        /// </summary>
        /// <param name="dataTable">The DataTable to convert to dictionaries.</param>
        /// <returns>A collection of dictionaries representing the rows of the DataTable.</returns>
        public IEnumerable<IDictionary<string, object>> xDataTableToDictionaries()
        {
            if (source.xIsEmpty()) return new List<IDictionary<string, object>>();
            return source.AsEnumerable().Select(row =>
                source.Columns.Cast<DataColumn>().ToDictionary(column => column.ColumnName, column => row[column])
            ).ToList();
        }
    }
}

public static class IDataReaderExtensions
{
    extension(IDataReader source)
    {
        public T xToEntity<T>(Action<T> action = null) where T : class, new()
        {
            var properties = typeof(T).GetProperties().ToList();
            var newItem = new T();

            Enumerable.Range(0, source.FieldCount - 1).xForEach(i =>
            {
                if (!source.IsDBNull(i))
                {
                    var property = properties.Where(m => m.Name.Equals(source.GetName(i))).FirstOrDefault();
                    if (!property.xIsEmpty())
                        if (source.GetFieldType(i) == property.PropertyType)
                            property.SetValue(newItem, source[i]);
                }
            });

            if (action.xIsNotEmpty()) action(newItem);

            return newItem;
        }
    }
}
