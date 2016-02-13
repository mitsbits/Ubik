using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ubik.Infra.Contracts;

namespace Ubik.Infra.Hierarchies
{
    public abstract class BaseHierarchy<TKey> : IHierarchicalEnumerable where TKey : struct
    {
        private readonly ICollection<IHasParent<TKey>> _source;

        protected BaseHierarchy()
        {
            _source = new HashSet<IHasParent<TKey>>();
        }

        protected internal ICollection<IHasParent<TKey>> Source => _source;

        public virtual IEnumerator GetEnumerator()
        {
            return _source.Where(x => x.Depth == _source.Min(s => s.Depth)).GetEnumerator();
        }

        public IHierarchyData GetHierarchyData(object enumeratedItem)
        {
            return enumeratedItem as IHierarchyData;
        }
    }
}