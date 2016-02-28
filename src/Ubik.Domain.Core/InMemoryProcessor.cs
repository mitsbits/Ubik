using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Domain.Core
{
    public class InMemoryProcessor : ICommandBus
    {
        private readonly Dictionary<Type, List<Func<ICommand, ICommandResult>>> _routes = new Dictionary<Type, List<Func<ICommand, ICommandResult>>>();


        public void RegisterHandler<T>(Func<T, ICommandResult> handler) where T : ICommand
        {
            List<Func<ICommand, ICommandResult>> handlers;
            if (!_routes.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<Func<ICommand, ICommandResult>>();
                _routes.Add(typeof(T), handlers);
            }
            handlers.Add(DelegateAdjuster.CastArgument<ICommand, T>(x => handler(x)));
        }

        public Task<ICommandResult> Process<TCommand>(TCommand command) where TCommand : ICommand
        {
            List<Func<ICommand, ICommandResult>> handlers;
            if (_routes.TryGetValue(typeof(TCommand), out handlers))
            {
                if (handlers.Count != 1) throw new InvalidOperationException("cannot send to more than one handler");
                return Task.FromResult(handlers[0].Invoke(command));
            }
            else
            {
                throw new InvalidOperationException("no handler registered");
            }
        }

    }
}
