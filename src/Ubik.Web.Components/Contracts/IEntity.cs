namespace Ubik.Web.Components.Contracts
{
    internal interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}