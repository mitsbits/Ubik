using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Domain.Core;
using Ubik.Web.Client.Backoffice.Events;
using Ubik.Web.Membership;
using Ubik.Web.Membership.Events;

namespace Ubik.Web.Client.Backoffice.EventHandlers
{
    public class RolePersistedHandler : IHandlesMessage<RoleRowStateChanged>
    {
        private readonly IEventBus _eventBus;

        public RolePersistedHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        public async Task Handle(RoleRowStateChanged message)
        {
          await  _eventBus.Publish(new CacheDataBecameStale(Constants.RoleViewModelsCacheKey));
        }
    }
}
