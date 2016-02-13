namespace Ubik.Web.Components.Contracts
{
    public interface IComponent
    {
        ComponentStateFlavor StateFlavor { get; }

        void SetState(ComponentStateFlavor flavor);
    }

    internal interface IComponentCanPublishSuspend : IComponent, ICanPublishSuspend { }

    internal interface IComponentCanBeDeleted : IComponent, ICanBeDeleted { }
}