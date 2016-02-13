namespace Ubik.Web.Basis.Navigation.Contracts
{
    public interface INavigationGroup
    {
        string Display { get; }
        string Key { get; }
        string Description { get; }
    }
}