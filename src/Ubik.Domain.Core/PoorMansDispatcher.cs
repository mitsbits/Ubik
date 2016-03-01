using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Domain.Core
{
    public class PoorMansDispatcher : IDispatcherInstance, IEventBus, ICommandBus
    {
        private readonly IServiceProvider _provider;

        public PoorMansDispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        #region ICommandBus

        public async Task<ICommandResult> Process<TCommand>(TCommand command) where TCommand : ICommand
        {
            var type = typeof(IHandlesCommand<>).MakeGenericType(command.GetType());
            var collectionType = typeof(IEnumerable<>).MakeGenericType(type);
            var hit = _provider.GetService(collectionType);
            if (hit == null) return await Task.FromResult(CommandResult.Create(false, string.Format("no command precessor for {0}", nameof(command))));

            var collection = hit as IEnumerable<dynamic>;
            if (collection.Count() > 1) return await Task.FromResult(CommandResult.Create(false, string.Format("multiple command precessors for {0}", nameof(command))));
            var handler = collection.Single();

            Task<ICommandResult> task = handler.Execute(command);

            return await task;
        }

        #endregion ICommandBus

        #region IEventBus

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

        #endregion IEventBus

        #region IDispatcherInstance

        public Task Stop()
        {
            return Task.FromResult(0);
        }

        #endregion IDispatcherInstance
    }
}