using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Domain.Core.NServiceBus
{

    public class DefaultDispatcher : IDispatcherInstance, IEventBus
    {
        IBus _endpoint;

        public DefaultDispatcher(IBus endpoint) { _endpoint = endpoint; }
        public  Task Publish(IEvent @event)
        {
          _endpoint.Publish(@event);
            return Task.FromResult(0);

        }

        public Task Stop()
        {
            _endpoint?.Dispose();
            return Task.FromResult(0);
        }
    }
}
