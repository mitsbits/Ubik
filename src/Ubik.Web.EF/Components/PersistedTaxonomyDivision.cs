using System.Collections.Generic;
using Ubik.EF6.Contracts;

namespace Ubik.Web.EF.Components
{
    public class PersistedTaxonomyDivision : ISequenceBase
    {
        public PersistedTaxonomyDivision()
        {
            Elements = new HashSet<PersistedTaxonomyElement>();
        }

        public int Id { get; set; }
        public virtual ICollection<PersistedTaxonomyElement> Elements { get; set; }
        public int TextualId { get; set; }
        public virtual PersistedTextual Textual { get; set; }
    }
}