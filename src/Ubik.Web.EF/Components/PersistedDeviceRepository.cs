using Mehdime.Entity;
using System;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ubik.EF6;
using Ubik.Infra.Contracts;

namespace Ubik.Web.EF.Components
{
    public class PersistedDeviceRepository : ReadWriteRepository<PersistedDevice, ComponentsDbContext>, ICRUDRespoditory<PersistedDevice>
    {
        public PersistedDeviceRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }

        public override async Task<PersistedDevice> GetAsync(Expression<Func<PersistedDevice, bool>> predicate)
        {
            var db = DbContext.Set<PersistedDevice>();
            return await db.Include("Sections").Include("Sections.Slots").FirstOrDefaultAsync(predicate);
        }
    }
}