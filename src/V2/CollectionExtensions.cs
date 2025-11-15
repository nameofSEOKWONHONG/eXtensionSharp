using System;

namespace eXtensionSharp;

public static class CollectionExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        public IEnumerable<T> xDistinct()
        {
            if(source.xIsEmpty()) return Array.Empty<T>();

            var lst = new List<T>();
            var items = source.xBatch(1000);
            items.xForEach(item =>
            {
                lst.AddRange(item.Distinct());
            });
            
            return lst;
        }

        public bool xTryDuplicate(out T v)
        {
            v = default;

            if (source.xIsEmpty()) return false;

            HashSet<T> set = new();
            foreach (var item in source)
            {
                if (!set.Add(item))
                {
                    v = item;
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<T[]> xBatch(int batchSize)
        {
            var array = source.ToArray();
            for (int i = 0; i < array.Length; i += batchSize)
            {
                int size = Math.Min(batchSize, array.Length - i);
                var batch = array[i..(i + size)];
                yield return batch;
            }
        }

        /// <summary>
        ///     foreach loop
        /// </summary>
        /// <param name="action"></param>
        public void xForEach(Action<T, int> action)
        {
            if (source.xIsEmpty()) return;
            
            int i = 0;
            foreach (var item in source)
            {
                action(item, i);
                i++;
            }
        }

        /// <summary>
        ///     foreach loop
        /// </summary>
        /// <param name="action"></param>
        public void xForEach(Action<T> action)
        {
            if (source.xIsEmpty()) return;
            
            source.xForEach((item, i) => { action(item); });
        }



        public void xForEach(Func<T, int, bool> func)
        {
            var i =0;
            source.xForEach(item =>
            {
                var isBreak = !func(item, i);
                i += 1;
                if (isBreak) return false;
                return true;
            });
        }

        /// <summary>
        ///     use class, allow break;
        /// </summary>
        /// <param name="func"></param>
        public void xForEach(Func<T, bool> func)
        {
            if (source.xIsEmpty()) return;
            
            source.xForEach((item, i) =>
            {
               var @break = func(item);
               if(@break) return false; 

               return true;
            });
        }

        public async Task xForEachAsync(Func<T, Task> func)
        {
            if (source.xIsEmpty()) return;

            foreach (var value in source)
            {
                await func(value);
            }
        }

        /// <summary>
        /// Parellel ForEach
        /// </summary>
        /// <param name="func"></param>
        /// <param name="options">ParallelOptions</param>
        /// <returns></returns>
        public async Task<bool> xParallelForEachAsync(ParallelOptions options, Func<T, CancellationToken, Task> func)
        {
            if (options.xIsEmpty())
            {
                options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount),
                    CancellationToken = default
                };
            }

            //need to be sure this is the correct way to do it.
            try
            {
                await Parallel.ForEachAsync(source, options, async (item, token) =>
                {
                    token.ThrowIfCancellationRequested();

                    await func(item, token);
                });
            }
            catch (OperationCanceledException) 
            { 
                return true;
            }

            return false;
        }

        /// <summary>
        /// same method at python product method.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="item3"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="Exception"></exception>
        public void xForEach(T[] item2, T[] item3, Action<T, T, T> action)
        {
            var lst = source.ToList();
            if(lst.Count < item2.Length || 
               lst.Count < item3.Length) 
                throw new Exception("item array are not same length");
            
            for (var i = 0; i < lst.Count; i++)
            {
                action(lst[i], item2[i], item3[i]);
            }
        }


        




  
    }
}

