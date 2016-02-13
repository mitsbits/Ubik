using Ubik.Infra.Contracts;

namespace Ubik.Web.EF.Components
{
    public class PersistedTaxonomyElementRecursion : IHasParent<int>
    {
        public int AncestorId { get; set; }
        public int ParentId { get; set; }

        public int Id { get; set; }

        public int Depth { get; set; }

        public virtual PersistedTaxonomyElement TaxonomyElement { get; set; }
    }
}