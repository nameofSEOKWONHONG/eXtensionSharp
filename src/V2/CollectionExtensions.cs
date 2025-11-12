using System;

namespace eXtensionSharp.V2;

public static class CollectionExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        public IEnumerable<T> xDistinct()
        {
            if(source.xIsEmpty()) return Array.Empty<T>();

            var hash = new HashSet<T>();
            source.xForEach(item => { hash.Add(item); });

            return hash;
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
    }
}
