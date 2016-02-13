using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Ubik.Infra.Contracts;

namespace Ubik.Cache.Runtime
{
    public class MemoryDefaultCacheProvider : ICacheProvider
    {


        private readonly TimeSpan _aLongTime = TimeSpan.FromDays(365 * 1000);
        protected static MemoryCache _cache;
        protected object _lock = new object();

        protected virtual MemoryCache CurrentCache
        {
            get
            {
                lock (_lock)
                {
                   
                    if (_cache == null)
                        _cache = new MemoryCache(new MemoryCacheOptions());
                }
                return _cache;
            }
        }

        public virtual object GetItem(string key)
        {
            return CurrentCache.Get(key.ToLowerInvariant());
        }

        public virtual void SetItem(string key, object value)
        {
            //TODO: add guards
            if (value == null) return;
            if (string.IsNullOrWhiteSpace(key)) return;
            lock (_lock)
            {
                CurrentCache.Set(key.ToLowerInvariant(), value, new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = _aLongTime });
            }
        }

        public virtual void SetItem(string key, object value, params int[] ttl)
        {
            if (value == null) return;
            var ttlCount = (ttl.Count() > 4) ? 4 : ttl.Count();
            var cacheDur = DateTimeOffset.Now;
            for (var i = 0; i < ttlCount; i++)
            {
                switch (i)
                {
                    case 3:
                        cacheDur = cacheDur.AddSeconds(ttl[0]);
                        break;

                    case 2:
                        cacheDur = cacheDur.AddMinutes(ttl[1]);
                        break;

                    case 1:
                        cacheDur = cacheDur.AddHours(ttl[2]);
                        break;

                    case 0:
                        cacheDur = cacheDur.AddDays(ttl[3]);
                        break;
                }
            }

            lock (_lock)
            {
                CurrentCache.Set(key.ToLowerInvariant(), value, new MemoryCacheEntryOptions() { AbsoluteExpiration = cacheDur });
            };
               
           
        }

        public virtual void SetItem(string key, object value, DateTime absoluteExpiration)
        {
            if (value == null) return;
            lock (_lock)
            {
                CurrentCache.Set(key.ToLowerInvariant(), value, new MemoryCacheEntryOptions() { AbsoluteExpiration = absoluteExpiration });
            }
        }

        public virtual void RemoveItem(string key)
        {
           
            lock (_lock)
            {
                CurrentCache.Remove(key.ToLowerInvariant());
            }
        }
    }
}