using Mehdime.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Ubik.EF6;
using Ubik.Web.EF.Components.Contracts;

namespace Ubik.Web.EF.Components
{
    public class PersistedTaxonomyDivisionRepository : ReadWriteRepository<PersistedTaxonomyDivision, ComponentsDbContext>, IPersistedTaxonomyDivisionRepository
    {
        public PersistedTaxonomyDivisionRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }

        public async Task UpdateHierarchy(int id)
        {
            var projections = await DbContext.Set<PersistedTaxonomyElement>().Where(x => x.DivisionId == id).ToListAsync();
            var roots = projections.Where(x => x.ParentId == 0);
            foreach (var root in roots)
            {
                UpdateElementPlacement(root, 1, ref projections);
            }
        }

        private static void UpdateElementPlacement(PersistedTaxonomyElement element, int level, ref List<PersistedTaxonomyElement> collection)
        {
            element.Depth = level;
            foreach (var persistedTaxonomyElement in collection.Where(x => x.ParentId == element.Id))
            {
                UpdateElementPlacement(persistedTaxonomyElement, level + 1, ref collection);
            }
        }
    }
}