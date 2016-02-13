namespace Ubik.Web.Components.Contracts
{
    public interface ICanPublishSuspend
    {
        void Publish();

        void Suspend();

        bool IsPublished { get; }
    }
}