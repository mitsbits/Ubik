using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ubik.Domain.Core
{
    public class PoorMansDispatcher : IDispatcherInstance, IEventBus
    {
        private readonly IServiceProvider _provider;

        public PoorMansDispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task Publish<T>(T @event) where T : IEvent
        {
            var type = typeof(IHandlesMessage<>).MakeGenericType(@event.GetType());
            var collectionType = typeof(IEnumerable<>).MakeGenericType(type);
            var hit = _provider.GetService(collectionType);
            if (hit == null) return;

            var collection = hit as IEnumerable<dynamic>;
            var tasks = new List<Task>();
            foreach (var handler in collection)
            {
                Task task = handler.Handle(@event);
                tasks.Add(task);
            }
            await Task.WhenAll(tasks.ToArray());
        }

        public Task Stop()
        {
            return Task.FromResult(0);
        }
    }
}