using Mehdime.Entity;
using System;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ubik.EF6;
using Ubik.EF6.Contracts;
using Ubik.Infra.Contracts;
using Ubik.Web.EF.Components.Contracts;
using Ubik.Web.Components.Domain;
using System.Linq;

namespace Ubik.Web.EF.Components
{
    public class PersistedSectionRepository : ReadWriteRepository<PersistedSection, ComponentsDbContext>, IPersistedSectionRepository
    {
        public PersistedSectionRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }

        public Task AddSlot(PersistedSlot slot)
        {
            DbContext.Set<PersistedSlot>().Add(slot);
            return Task.FromResult(0);
        }

        public override async Task<PersistedSection> GetAsync(Expression<Func<PersistedSection, bool>> predicate)
        {
            var db = DbContext.Set<PersistedSection>();
            return await db.Include("Slots").FirstOrDefaultAsync(predicate);
        }

        public async Task<int> GetNext()
        {
            var sepProvider = DbContext as ISequenceProvider;
            if (sepProvider == null)
                throw new ApplicationException(string.Format("{0} does not implement {1}",
                    nameof(DbContext), typeof(ISequenceProvider).AssemblyQualifiedName));
            return await sepProvider.GetNext(typeof(PersistedSection));
        }


    }
}