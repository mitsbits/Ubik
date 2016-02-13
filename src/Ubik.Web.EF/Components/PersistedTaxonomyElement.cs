using System.Collections.Generic;
using Ubik.EF6.Contracts;
using Ubik.Infra.Contracts;

namespace Ubik.Web.EF.Components
{
    public class PersistedTaxonomyElement : PersistedComponent, ISequenceBase, IHasParent<int>
    {
        private readonly ICollection<PersistedTaxonomyElementRecursion> _taxonomyElementRecursions;

        public PersistedTaxonomyElement()
        {
            _taxonomyElementRecursions = new HashSet<PersistedTaxonomyElementRecursion>();
        }

        public int ParentId { get; set; }
        public int Depth { get; set; }
        public int DivisionId { get; set; }
        public virtual PersistedTaxonomyDivision Division { get; set; }
        public int TextualId { get; set; }
        public virtual PersistedTextual Textual { get; set; }

        public virtual ICollection<PersistedTaxonomyElementRecursion> TaxonomyElementRecursions => _taxonomyElementRecursions;
    }
}