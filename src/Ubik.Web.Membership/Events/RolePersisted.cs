using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Domain.Core;

namespace Ubik.Web.Membership.Events
{
    public class RolePersisted :IEvent
    {
        private readonly string _key;
        public RolePersisted()
        {
            _key = Constants.RoleViewModelsCacheKey;
        }
        public Payload Payload()
        {
            return new Payload() { PayloadType = GetType().AssemblyQualifiedName, PayloadJson = Newtonsoft.Json.JsonConvert.SerializeObject(this) };
        }

        public string CacheKey { get { return _key; } }
    }
}
