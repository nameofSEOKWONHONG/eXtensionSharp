using System.Data;

namespace eXtensionSharp
{
    public static class XDataTableExtensions
    {
        #region [Datatable & DataReader]

        public static DataTable xToDataTable<T>(this IEnumerable<T> entities)
            where T : class
        {
            if (entities.xIsEmpty()) return new DataTable();
            
            var properties = entities.xFirst().xGetProperties();

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

        public static T xToEntity<T>(this IDataReader reader, Action<T> action = null)
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
                        if (reader.GetFieldType(i) == property.PropertyType)
                            property.SetValue(newItem, reader[i]);
                }
            });

            if (action.xIsNotEmpty()) action(newItem);

            return newItem;
        }

        #endregion [Datatable & DataReader]
    }
}