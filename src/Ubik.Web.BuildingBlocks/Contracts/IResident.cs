using System.Collections.Generic;
using System.Security.Claims;
using Ubik.Web.Basis.Navigation.Contracts;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.BuildingBlocks.Contracts
{
    public interface IResident
    {
        IResidentAdministration Administration { get; }
        IResidentSecurity Security { get; }
        IModuleDescovery Modules { get; }
        //IResidentPubSub PubSub { get; }
    }

    public interface IResidentSecurity
    {
        IEnumerable<Claim> Roles { get; }

        IEnumerable<Claim> ClaimsForRole(string role);
    }

    public interface IResidentAdministration
    {
        IMenuProvider<INavigationElements<int>> BackofficeMenu { get; }
    }

    public interface IModuleDescovery
    {
        IReadOnlyCollection<IModuleDescriptor> Installed { get; }
    }

    //public interface IResidentPubSub : IDomainCommandProcessor, IEventPublisher
    //{
    //}

    //public class ResidentPubSub : IResidentPubSub
    //{
    //    private readonly IDomainCommandProcessor _processor;
    //    private readonly IEventPublisher _publisher;

    //    public ResidentPubSub(IDomainCommandProcessor processor, IEventPublisher publisher)
    //    {
    //        _processor = processor;
    //        _publisher = publisher;
    //    }

    //    public void Process<TCommand>(TCommand command) where TCommand : IDomainCommand
    //    {
    //        _processor.Process(command);
    //    }

    //    public IEnumerable<TResult> Process<TCommand, TResult>(TCommand command) where TCommand : IDomainCommand
    //    {
    //        return _processor.Process<TCommand, TResult>(command);
    //    }

    //    public void Process<TCommand, TResult>(TCommand command, Action<TResult> resultHandler) where TCommand : IDomainCommand
    //    {
    //        _processor.Process(command, resultHandler);
    //    }

    //    public void Publish<T>(T @event) where T : IDomainEvent
    //    {
    //        _publisher.Publish(@event);
    //    }
    //}
}