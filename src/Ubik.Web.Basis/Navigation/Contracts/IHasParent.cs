namespace Ubik.Web.Basis.Navigation.Contracts
{
    public interface IHasParent<out TKey> where TKey : struct
    {
        TKey Id { get; }

        TKey ParentId { get; }
    }
}