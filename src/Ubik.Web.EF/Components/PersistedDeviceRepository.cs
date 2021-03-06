using Mehdime.Entity;
using System;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ubik.EF6;
using Ubik.EF6.Contracts;
using Ubik.Infra.Contracts;
using Ubik.Web.EF.Components.Contracts;

namespace Ubik.Web.EF.Components
{
    public class PersistedDeviceRepository : ReadWriteRepository<PersistedDevice, ComponentsDbContext>, IPersistedDeviceRepository
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

        public async Task<int> GetNext()
        {
            var sepProvider = DbContext as ISequenceProvider;
            if (sepProvider == null)
                throw new ApplicationException(string.Format("{0} does not implement {1}", 
                    nameof(DbContext), typeof(ISequenceProvider).AssemblyQualifiedName));
            return await sepProvider.GetNext(typeof(PersistedDevice));
        }
    }
}