using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace Meshop.Framework.Caching
{
    public class Cache
    {
        private readonly ObjectCache _cache;

        public Cache()
        {
            _cache = MemoryCache.Default;
        }

        public T Get<T>(string key)
        {
            return (T)_cache[key];
        }

        public object this[string key]
        {
            get { return _cache[key]; }
        } 

        public void Add(string key, object data, int cacheTime = 5)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy{ AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime) };
            _cache.Add(new CacheItem(key, data), policy);
        }

        public bool IsSet(string key)
        {
            return (_cache.Contains(key));
        }


        public void Remove(string key)
        {
            _cache.Remove(key);
        }


        public void Clear()
        {
            foreach (var item in _cache)
                Remove(item.Key);
        }
    }

    
}
