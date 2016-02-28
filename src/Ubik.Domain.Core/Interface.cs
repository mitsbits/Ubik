using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Domain.Core
{
    public interface IMessage { }

    public interface IEvent : IMessage, IPayload { }

    public interface IPayload
    {
        Payload Payload();
    }

    public class Payload
    {
      public string PayloadType
        {
            get; set;
        }
        public string PayloadJson
        {
            get; set;
        }
    }

    public interface ICommand : IMessage { }

    public interface IHandlesMessage<T>
    {
        Task Handle(T message);
    }

    public interface IEventBus
    {
        Task Publish<T>(T @event) where T :IEvent;
    }


    public interface ICommandBus
    {
        Task<ICommandResult> Process<TCommand>(TCommand command) where TCommand : ICommand;
    }


    public interface ICommandResult { }

    public interface IDispatcherInstance
    {
        Task Stop();
    }


}
