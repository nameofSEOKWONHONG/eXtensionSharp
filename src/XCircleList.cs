using System.Collections.Generic;

namespace eXtensionSharp
{
    public class CircleXList<T> : List<T>
        where T : class, new()
    {
        public CircleXList()
        {
        }

        public CircleXList(int capacity)
            : base(capacity)
        {
        }

        public CircleXList(IEnumerable<T> enumerable)
        {
            AddRange(enumerable);
        }

        public int Index { get; private set; }

        public T Next()
        {
            Index++;
            if (Index > Count - 1)
                Index = 0;
            else if (Index < 0) Index = 0;

            return base[Index];
        }

        public T Previous()
        {
            Index--;
            if (Index < 0)
                Index = 0;
            else if (Index > Count - 1) Index = 0;

            return base[Index];
        }
    }
}