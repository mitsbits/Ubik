using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Components
{
    public class EntityBase<TKey> : IEntity<TKey>
    {
        public TKey Id { get; protected set; }
    }
}