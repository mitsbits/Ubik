namespace Ubik.Web.Components.Contracts
{
    internal interface ITaxonomyElement<out TKey> : IEntity<TKey>
    {
        TKey ParentId { get; }
        int Depth { get; }
    }
}