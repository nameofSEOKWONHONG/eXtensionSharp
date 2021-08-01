using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Mapster;

namespace eXtensionSharp {
    /// <summary>
    /// 스레드 세이프 Dictionary pool
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class XDictionaryPool<TKey, TValue> : IDisposable{
        private readonly ConcurrentQueue<TKey> _keyQueue;
        private readonly ConcurrentDictionary<TKey, ConcurrentQueue<TValue>> _pool;
        private readonly byte _poolSize;
        private const byte DEFAULT_POOL_SIZE = 10;

        public XDictionaryPool() {
            _poolSize = DEFAULT_POOL_SIZE;
            _keyQueue = new ConcurrentQueue<TKey>();
            _pool = new ConcurrentDictionary<TKey, ConcurrentQueue<TValue>>(Environment.ProcessorCount * 2, _poolSize);
        }

        public XDictionaryPool(byte maxPoolSize) {
            if (maxPoolSize <= 0)
                maxPoolSize = (byte) Math.Min(Environment.ProcessorCount, byte.MaxValue);

            _poolSize = maxPoolSize;
            _keyQueue = new ConcurrentQueue<TKey>();
            _pool = new ConcurrentDictionary<TKey, ConcurrentQueue<TValue>>(Environment.ProcessorCount * 2, _poolSize);
        }

        public bool Add(TKey key, TValue value) {
            if (key.xIsNull())
                return false;

            if (!_pool.ContainsKey(key) && _pool.TryAdd(key, new ConcurrentQueue<TValue>())) {
                _keyQueue.Enqueue(key);

                while (_pool.Count > _poolSize) {
                    TKey localKey;
                    if (_keyQueue.TryDequeue(out localKey)) Remove(localKey);
                }
            }

            ConcurrentQueue<TValue> q;
            if (_pool.TryGetValue(key, out q)) {
                q.Enqueue(value);
                while (q.Count > _poolSize) {
                    TValue localValue;
                    q.TryDequeue(out localValue);
                }
            }
            else {
                return false;
            }

            return true;
        }

        public bool Remove(TKey key) {
            if (key.xIsNull())
                return false;

            ConcurrentQueue<TValue> value;
            return _pool.TryRemove(key, out value);
        }

        public int Count(TKey key) {
            if (key.xIsNull())
                return 0;

            if (!_pool.ContainsKey(key))
                _pool.TryAdd(key, new ConcurrentQueue<TValue>());

            ConcurrentQueue<TValue> q;
            if (_pool.TryGetValue(key, out q)) return q.Count;
            return 0;
        }

        /// <summary>
        /// Queue 이므로 조회시 삭제됨
        /// </summary>
        /// <param name="key"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public TValue GetValue(TKey key, Func<TValue> creator = null) {
            if (key.xIsNull())
                return default;

            if (!_pool.ContainsKey(key))
                _pool.TryAdd(key, new ConcurrentQueue<TValue>());

            ConcurrentQueue<TValue> q;
            if (_pool.TryGetValue(key, out q)) {
                TValue v;
                if (q.TryDequeue(out v))
                    return v;
                if (creator.xIsNotNull())
                    return creator();
            }

            return default;
        }

        public bool Release(TKey key, TValue value) {
            return Add(key, value); //just adds it back to key's queue
        }

        public void Clear() {
            _keyQueue.Clear();
            _pool.Clear();
        }

        public IDictionary<TKey, TValue> ToDictionary() {
            var dic = new Dictionary<TKey, TValue>();

            try {
                _pool.xForEach(key => {
                    key.Value.xForEach(value => {
                        dic.Add(key.Key, value);
                        return true;
                    });
                });
            }
            catch (Exception e) {
                throw new Exception("duplicate error", e);
            }

            return dic;
        }

        public void Dispose() {
            Clear();
        }
    }

    /// <summary>
    /// Dictionary Util
    /// </summary>
    public static class JDictionaryUtil {
        /// <summary>
        /// dictionary concat.
        /// if same key exists, throw error.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IDictionary<string, T> xConcat<T>(this IDictionary<string, T> first, IDictionary<string, T> second) {
            return first.Concat(second)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// dictionary concat and update.
        /// if same key exists, update from second value.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IDictionary<string, T> xConcatUpdate<T>(this IDictionary<string, T> first, IDictionary<string, T> second) {
            var result = new Dictionary<string, T>();
            first.xForEach(firstPair => {
                result.Add(firstPair.Key, firstPair.Value);
            });

            second.xForEach(secondPair => {
                var exists = result.ContainsKey(secondPair.Key);
                if (exists) {
                    if (result[secondPair.Key].xIsEmpty()) {
                        result[secondPair.Key] = secondPair.Value;
                    }
                }
                else {
                    result.Add(secondPair.Key, secondPair.Value);
                }
            });

            return result;
        }

        public static IDictionary<string, object> xToDictionary<T>(this T entity) where T : class {
            return entity.Adapt<Dictionary<string, object>>();
        }
    }
}