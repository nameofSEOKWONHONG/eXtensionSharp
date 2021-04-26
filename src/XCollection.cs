using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace eXtensionSharp {
    public static class XCollection {
        #region [for & foreach]

        /// <summary>
        ///     foreach loop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="action"></param>
        public static void xForEach<T>(this IEnumerable<T> iterator, Action<T> action) {
            if (iterator.xCount() > XConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({XConst.LOOP_WARNING_COUNT})");

            var index = 0;
            foreach (var item in iterator) {
                action(item);
                if (index % XConst.LOOP_LIMIT == 0)
                    XConst.SetInterval(XConst.SLEEP_INTERVAL);

                index++;
            }
        }

        /// <summary>
        /// for loop
        /// </summary>
        /// <param name="iterator"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        public static void xFor<T>(this IEnumerable<T> iterator, Action<T> action) {
            if (iterator.xCount() > XConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({XConst.LOOP_WARNING_COUNT})");
            
            var srcs = iterator.ToArray();
            for (var i = 0; i < srcs.Length; i++) {
                action(srcs[i]);
                if (i % XConst.LOOP_LIMIT == 0)
                    XConst.SetInterval(XConst.SLEEP_INTERVAL);
            }
        }

        /// <summary>
        ///     foreach loop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="action"></param>
        public static void xForEach<T>(this IEnumerable<T> iterator, Action<T, int> action) {
            if (iterator.xCount() > XConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({XConst.LOOP_WARNING_COUNT})");

            var index = 0;
            iterator.xForEach(item => {
                action(item, index);
                index++;
            });
        }

        public static void xFor<T>(this IEnumerable<T> iterator, Action<T, int> action) {
            if (iterator.xCount() > XConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({XConst.LOOP_WARNING_COUNT})");

            var index = 0;
            iterator.xFor(item => {
                action(item, index);
                index++;
            });
        }

        /// <summary>
        ///     use class, allow break;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="func"></param>
        public static void xForEach<T>(this IEnumerable<T> iterator, Func<T, bool> func) {
            if (iterator.xCount() > XConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({XConst.LOOP_WARNING_COUNT})");

            var index = 0;
            foreach (var item in iterator) {
                var isBreak = !func(item);
                if (isBreak) break;
                
                if (index % XConst.LOOP_LIMIT == 0)
                    XConst.SetInterval(XConst.SLEEP_INTERVAL);
                
                index++;
            }
        }
        
        public static void xForEach<T>(this IEnumerable<T> iterator, Func<T, int, bool> func) {
            if (iterator.xCount() > XConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({XConst.LOOP_WARNING_COUNT})");

            var index = 0;
            iterator.xForEach(item => {
                var isBreak = !func(item, index);
                if(isBreak) return false;
                
                if (index % XConst.LOOP_LIMIT == 0)
                    XConst.SetInterval(XConst.SLEEP_INTERVAL);

                return true;
            });
        }

        public static void xFor<T>(this IEnumerable<T> iterator, Func<T, int, bool> func) {
            if (iterator.xCount() > XConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({XConst.LOOP_WARNING_COUNT})");
            
            var srcs = iterator.ToArray();
            for (var i = 0; i < srcs.Length; i++) {
                var isBreak = !func(srcs[i], i);
                if(isBreak) break;
                
                if (i % XConst.LOOP_LIMIT == 0)
                    XConst.SetInterval(XConst.SLEEP_INTERVAL);
            }
        }

        public static async Task xForEachAsync<T>(this IEnumerable<T> iterator, Func<T, Task> func) {
            foreach (var value in iterator)
            {
                await func(value);
            }
        }

        public static void xForEach(this (DateTime from, DateTime to) fromToDate, ENUM_DATETIME_FOREACH_TYPE type,  Action<DateTime> action) {
            if (type.xIsEquals(ENUM_DATETIME_FOREACH_TYPE.DAY)) {
                for (var i = fromToDate.from; i <= fromToDate.to; i = i.AddDays(1)) {
                    action(i);
                }       
            }
            else if (type.xIsEquals(ENUM_DATETIME_FOREACH_TYPE.MONTH)) {
                for (var i = fromToDate.from; i <= fromToDate.to; i = i.AddMonths(1)) {
                    action(i);
                }
            }
            else {
                for (var i = fromToDate.from; i <= fromToDate.to; i = i.AddYears(1)) {
                    action(i);
                }
            }
        }

        #endregion [for & foreach]

        #region [Datatable & DataReader]

        public static DataTable xToDateTable<T>(this IEnumerable<T> entities)
            where T : class, new() {
            var entity = new T();
            var properties = entity.GetType().GetProperties();

            var dt = new DataTable();
            foreach (var property in properties) dt.Columns.Add(property.Name, property.PropertyType);

            entities.xForEach(item => {
                var itemProperty = item.GetType().GetProperties();
                var row = dt.NewRow();
                foreach (var property in itemProperty) row[property.Name] = property.GetValue(item);
                dt.Rows.Add(row);
                return true;
            });

            return dt;
        }

        public static T xFromReaderToObject<T>(this IDataReader reader)
            where T : class, new() {
            var properties = typeof(T).GetProperties().xToList();

            var newItem = new T();

            Enumerable.Range(0, reader.FieldCount - 1).xForEach(i => {
                if (!reader.IsDBNull(i)) {
                    var property = properties.Where(m => m.Name.Equals(reader.GetName(i))).xFirst();
                    if (property.xIsNotNull())
                        if (reader.GetFieldType(i).Equals(property.PropertyType))
                            property.SetValue(newItem, reader[i]);
                }
            });

            return newItem;
        }

        #endregion [Datatable & DataReader]
    }
    
    public class ENUM_DATETIME_FOREACH_TYPE : XENUM_BASE<ENUM_DATETIME_FOREACH_TYPE> {
        public static readonly ENUM_DATETIME_FOREACH_TYPE DAY = Define("Day");
        public static readonly ENUM_DATETIME_FOREACH_TYPE MONTH = Define("Month");
        public static readonly ENUM_DATETIME_FOREACH_TYPE YEAR = Define("Year");
    }
}