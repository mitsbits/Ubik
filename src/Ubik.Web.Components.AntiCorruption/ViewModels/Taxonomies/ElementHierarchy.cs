using System.Collections.Generic;
using System.Linq;
using Ubik.Infra.Contracts;
using Ubik.Infra.Hierarchies;
using Ubik.Web.EF.Components;

namespace Ubik.Web.Components.AntiCorruption.ViewModels.Taxonomies
{
    public class ElementHierarchicalNode : BaseHierarchalNode<int>
    {
        public ElementHierarchicalNode(IEnumerable<PersistedTaxonomyElement> source) : base(source)
        {
        }

        public ElementHierarchicalNode(ElementHierarchy container) : base(container)
        {
        }

        public override object Item
        {
            get { return Source.FirstOrDefault(x => x.Id == Id); }
        }

        public override IHierarchicalEnumerable Children
        {
            get { return new ElementHierarchy(Source.Where(x => x.ParentId == Id).Cast<PersistedTaxonomyElement>().ToList()); }
        }
    }

    public class ElementHierarchy : BaseHierarchy<int>
    {
        public ElementHierarchy(IEnumerable<PersistedTaxonomyElement> source) : base()
        {
            foreach (var persistedTaxonomyElement in source)
            {
                Source.Add(persistedTaxonomyElement);
            }
        }
    }
}