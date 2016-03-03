using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Domain.Core;
using Ubik.Infra.Contracts;
using Ubik.Web.Client.Backoffice.Events;

namespace Ubik.Web.Client.Backoffice.EventHandlers
{
    public class CacheDataBecameStaleHandler : IHandlesMessage<CacheDataBecameStale>
    {
        private readonly ICacheProvider _cache;

        public CacheDataBecameStaleHandler(ICacheProvider cache)
        {
            _cache = cache;
        }

        public  Task Handle(CacheDataBecameStale message)
        {
            _cache.RemoveItem(message.Key);
            return Task.FromResult(0);
        }
    }
}
