using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ubik.Domain.Core
{
    public class InMemoryPublisher : IEventBus
    {
        private readonly Dictionary<Type, List<Action<IEvent>>> _routes = new Dictionary<Type, List<Action<IEvent>>>();

        public Task Publish<T>(T @event) where T : IEvent
        {
            List<Action<IEvent>> handlers;
            if (!_routes.TryGetValue(@event.GetType(), out handlers)) return Task.FromResult(false);
            foreach (var handler in handlers)
            {
                var handler1 = handler;
                Task.Run(() =>
                {
                    handler1.Invoke(@event);
                });
                //dispatch on thread pool for added awesomeness
                //ThreadPool.QueueUserWorkItem(x => handler1(@event));
            }
            return Task.FromResult(true);
        }

        public void RegisterHandler<T>(Action<T> handler) where T : IEvent
        {
            List<Action<IEvent>> handlers;
            if (!_routes.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<Action<IEvent>>();
                _routes.Add(typeof(T), handlers);
            }
            handlers.Add(DelegateAdjuster.CastArgument<IEvent, T>(x => handler(x)));
        }
    }


}
