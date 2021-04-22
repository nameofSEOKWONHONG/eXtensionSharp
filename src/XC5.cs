using System.Collections.Generic;
using C5;

namespace eXtensionSharp {
    public class XList<T> : ArrayList<T> {
        public XList() : base(100) {
        }

        public XList(int capacity)
            : base(capacity) {
        }

        public XList(int capacity, MemoryType memoryType)
            : base(capacity, memoryType) {
        }

        public XList(IEnumerable<T> iterator) {
            AddAll(iterator);
        }
    }

    public class XLKList<T> : C5.LinkedList<T> {
        public XLKList() {
            
        }
    }

    public class XHList<T> : HashedArrayList<T> {
        public XHList() {
        }

        public XHList(int capacity) : base(capacity) {
        }

        public XHList(int capacity, MemoryType memoryType) : base(capacity, memoryType) {
        }
    }

    public class XHLKList<T> : HashedLinkedList<T> {
        public XHLKList() {
        }

        public XHLKList(MemoryType memoryType) : base(memoryType) {
        }
    }

    public class XHDictionary<K, V> : HashDictionary<K, V> {
    }

    public class XTDictionary<K, V> : TreeDictionary<K, V> {
    }
}