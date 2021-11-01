using System.Collections.Generic;

namespace eXtensionSharp
{
    public class CircleXList<T> : List<T>
        where T : class, new()
    {
        private int _index;
        public int Index => _index;
        
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


        public T Next()
        {
            _index++;
            if (_index > this.Count - 1)
                _index = 0;
            else if (_index < 0) _index = 0;

            return base[_index];
        }

        public T Previous()
        {
            _index--;
            if (_index < 0)
                _index = 0;
            else if (_index > this.Count - 1) _index = 0;

            return base[_index];
        }
    }
}