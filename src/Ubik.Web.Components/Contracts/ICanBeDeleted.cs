namespace Ubik.Web.Components.Contracts
{
    public interface ICanBeDeleted
    {
        void Delete();

        bool IsDeleted { get; }
    }
}