using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NetFabric.Hyperlinq;

namespace eXtensionSharp {
    public class CacheResolverFactory {
        private static Lazy<CacheResolverFactory> _instance = 
            new Lazy<CacheResolverFactory>(() => new CacheResolverFactory());

        public static CacheResolverFactory Instance {
            get
            {
                return _instance.Value;
            }
        }

        private XConcurrentCache<string> _caches = new();
        
        private CacheResolverFactory() {
            
        }

        public TEntity Execute<TResolver, TEntity>() where TResolver : CacheResolverBase<TEntity>, new() {
            TResolver resolver = new TResolver();
            
            var key = resolver.InitKey();
            if (_caches.Get(key).xIsEmpty()) {
                var valueObj = resolver.GetOrSet();
                if (_caches.Add(key, valueObj.xObjectToJson())) {
                    return valueObj;
                }
            }

            var cached = _caches.Get(key);
            var isReset = (DateTime.Now - cached.CachedDateTime).TotalSeconds > resolver.GetResetInterval();
            if (isReset) {
                _caches.Delete(key);
            }

            return cached.ValueObject.xJsonToObject<TEntity>();
        }

        public int Count() {
            return _caches.Count();
        }
    }

    public abstract class CacheResolverBase<T> {

        public abstract string InitKey();

        public abstract T GetOrSet();

        public abstract int GetResetInterval();

        public abstract void Delete();
    }
    
    public class XConcurrentCache<T> {
        private ConcurrentDictionary<string, CacheValue<T>> _cacheMaps;

        public XConcurrentCache() {
            _cacheMaps = new ConcurrentDictionary<string, CacheValue<T>>();
        }

        public CacheValue<T> Get(string key) {
            CacheValue<T> exists = default;
            _cacheMaps.TryGetValue(key, out exists);
            return exists;
        }

        public bool Add(string key, T addItem) {
            return _cacheMaps.TryAdd(key, new CacheValue<T>(addItem));
        }

        public bool Update(string key, T updateItem) {
            CacheValue<T> exists = default;
            if (!_cacheMaps.TryGetValue(key, out exists)) {
                return false;
            }

            return _cacheMaps.TryUpdate(key, new CacheValue<T>(updateItem), exists);
        }

        public bool Delete(string key) {
            CacheValue<T> exists = default;
            return _cacheMaps.TryRemove(key, out exists);
        }

        public int Count() {
            return _cacheMaps.Count;
        }
        
        public class CacheValue<T> {
            public T ValueObject { get; set; }
            public DateTime CachedDateTime { get; set; }

            public CacheValue(T value) {
                this.ValueObject = value;
                this.CachedDateTime = DateTime.Now;
            }
        }
    }
}