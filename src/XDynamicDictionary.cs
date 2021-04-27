using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace eXtensionSharp {
	public class DynamicDictionary<T> : DynamicObject, IDictionary<string, T>
	{
		IDictionary<string, T> _inner;

		#region Dictionary
		public DynamicDictionary(int capacity)
		{
			_inner = new Dictionary<string, T>(capacity);
		}
		public DynamicDictionary(IEqualityComparer<string> comparer)
		{
			_inner = new Dictionary<string, T>(comparer);
		}
		public DynamicDictionary(int capacity,IEqualityComparer<string> comparer)
		{
			_inner = new Dictionary<string,T>(capacity, comparer);
		}
		public DynamicDictionary(IDictionary<string,T> dictionary)
		{
			_inner = new Dictionary<string, T>(dictionary);
		}
		public DynamicDictionary(IDictionary<string, T> dictionary,IEqualityComparer<string> comparer)
		{
			_inner = new Dictionary<string, T>( dictionary,comparer);
		}
		public DynamicDictionary()
		{
			_inner = new Dictionary<string, T>();
		}

		public T this[string key] { get => _inner[key]; set => _inner[key] = value; }

		public ICollection<string> Keys => _inner.Keys;

		public ICollection<T> Values => _inner.Values;

		public int Count => _inner.Count;

		bool ICollection<KeyValuePair<string,T>>.IsReadOnly => _inner.IsReadOnly;

		public void Add(string key, T value)
		{
			_inner.Add(key, value);
		}

		void ICollection<KeyValuePair<string, T>>.Add(KeyValuePair<string, T> item)
		{
			_inner.Add(item);
		}

		public void Clear()
		{
			_inner.Clear();
		}

		bool ICollection<KeyValuePair<string, T>>.Contains(KeyValuePair<string, T> item)
		{
			return _inner.Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return _inner.ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
		{
			_inner.CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
		{
			return _inner.GetEnumerator();
		}

		public bool Remove(string key)
		{
			return _inner.Remove(key);
		}

		bool ICollection<KeyValuePair<string, T>>.Remove(KeyValuePair<string, T> item)
		{
			return _inner.Remove(item);
		}

		public bool TryGetValue(string key, out T value)
		{
			return _inner.TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _inner.GetEnumerator();
		}
		#endregion

		#region Dynamic
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			T res;
			if (_inner.TryGetValue(binder.Name, out res))
			{
				result = res;
				return true;
			}
			result = null;
			return false;
		}
		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			_inner[binder.Name] = (T)value;
			return true;
		}
		#endregion
	}
}