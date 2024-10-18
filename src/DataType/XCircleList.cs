using System.Collections.Generic;

namespace eXtensionSharp
{
    public class CircleList<T> : List<T>
    {
        public int Position
        {
            get => _index;
            set => _index = value;
        }
        
        private int _index = -1;

        public CircleList() { }

        public CircleList(int capacity) : base(capacity) { }

        public CircleList(IEnumerable<T> enumerable) : base(enumerable) { }

        /// <summary>
        /// Returns the next item in the list, wrapping around if necessary.
        /// </summary>
        public T Next()
        {
            if (_index > this.Count -1) _index = 0;
            _index = (_index + 1) % this.Count;
            return this[_index];
        }

        /// <summary>
        /// Returns the previous item in the list, wrapping around if necessary.
        /// </summary>
        public T Previous()
        {
            if(_index < 0) _index = this.Count - 1;
            else _index = (_index - 1 + this.Count) % this.Count;
            return this[_index];
        }
    }
}