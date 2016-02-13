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

    public interface IHandleMessages<T>
    {
        Task Handle(T message);
    }

    public interface IEventBus
    {
        Task Publish(IEvent @event);
    }

    public interface ICommandBus
    {
        Task<ICommandResult> Submit(ICommand @event);
    }


    public interface ICommandResult { }

    public interface IDispatcherInstance
    {
        Task Stop();
    }


}
