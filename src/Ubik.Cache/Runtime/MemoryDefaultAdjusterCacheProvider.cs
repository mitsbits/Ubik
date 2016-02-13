using System;
using System.Collections.Generic;
using System.Linq;
using Ubik.Infra.Contracts;

namespace Ubik.Cache.Runtime
{
    public class MemoryDefaultAdjusterCacheProvider : MemoryDefaultCacheProvider, IKeyAdjuster
    {
        private readonly IEnumerable<IKeyAdjusterProvider> _providers;

        public MemoryDefaultAdjusterCacheProvider(IEnumerable<IKeyAdjusterProvider> providers)
        {
            _providers = providers;
        }

        public IEnumerable<IKeyAdjusterProvider> Providers
        {
            get { return _providers; }
        }

        public string AdjustKey<TEnum>(TEnum t, params string[] values) where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            var provider = Providers.FirstOrDefault(x => x.Flavor == typeof(TEnum));
            return provider != null ? provider.AdjustKey(t, values) : string.Join("_", values);
        }
    }
}