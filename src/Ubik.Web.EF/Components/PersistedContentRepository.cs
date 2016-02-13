using Mehdime.Entity;
using Ubik.EF6;

namespace Ubik.Web.EF.Components
{
    internal class PersistedContentRepository : ReadWriteRepository<PersistedContent, ComponentsDbContext>
    {
        public PersistedContentRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }
    }
}