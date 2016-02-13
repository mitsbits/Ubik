using System.Collections.Generic;
using System.Linq;
using Ubik.Infra.Contracts;

namespace Ubik.Infra.Hierarchies
{
    public abstract class BaseHierarchalNode<TKey> : IHasParent<TKey>, IHierarchyData where TKey : struct
    {
        private readonly IEnumerable<IHasParent<TKey>> _source;

        public IHierarchicalEnumerable Container { get; set; }

        protected BaseHierarchalNode(IEnumerable<IHasParent<TKey>> source)
        {
            _source = source;
        }

        protected BaseHierarchalNode(BaseHierarchy<TKey> container)
        {
            _source = container.Source;
            Container = container;
        }

        public TKey Id { get; protected set; } = default(TKey);
        public TKey ParentId { get; protected set; } = default(TKey);
        public int Depth { get; protected set; } = default(int);

        public bool HasChildren
        {
            get { return Source.Any(x => !x.ParentId.Equals(default(TKey)) && x.ParentId.Equals(Id)); }
        }

        public abstract object Item { get; }

        public virtual string Type => GetType().FullName;

        public abstract IHierarchicalEnumerable Children { get; }

        public virtual IHierarchyData Parent
        {
            get { return Source.FirstOrDefault(x => x.Id.Equals(ParentId)) as IHierarchyData; }
        }

        protected virtual IEnumerable<IHasParent<TKey>> Source => _source;
    }
}