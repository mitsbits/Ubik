//using Microsoft.Extensions.Caching.Memory;
//using System.Collections.Specialized;
//using System.Runtime.Caching;

//namespace Ubik.Cache.Runtime
//{
//    public class MemoryNamedCacheProvider : MemoryDefaultCacheProvider
//    {
//        private readonly string _name;
//        private readonly NameValueCollection _config;

//        public MemoryNamedCacheProvider(string name, NameValueCollection config = null)
//            : base()
//        {
//            _name = name;
//            _config = config;
//        }

//        protected override MemoryCache CurrentCache
//        {
//            get
//            {
//                lock (_lock)
//                {
//                    if (_cache == null)
//                        _cache = new MemoryCache( new MemoryCacheOptions() { }
//                }
//                return _cache;
//            }
//        }
//    }
//}