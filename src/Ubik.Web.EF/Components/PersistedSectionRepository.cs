using Mehdime.Entity;
using System;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ubik.EF6;
using Ubik.Infra.Contracts;

namespace Ubik.Web.EF.Components
{
    public class PersistedSectionRepository : ReadWriteRepository<PersistedSection, ComponentsDbContext>, ICRUDRespoditory<PersistedSection>
    {
        public PersistedSectionRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }

        public override async Task<PersistedSection> GetAsync(Expression<Func<PersistedSection, bool>> predicate)
        {
            var db = DbContext.Set<PersistedSection>();
            return await db.Include("Slots").FirstOrDefaultAsync(predicate);
        }
    }
}