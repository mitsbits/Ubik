using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Domain.Core;

namespace Ubik.Web.Client.Backoffice.Events
{
    public class CacheDataBecameStale :IEvent
    {
        private string _key;

        public CacheDataBecameStale(string key)
        {
            _key = key;
        }
        public string Key { get { return _key; } }

    }
}
