using Mehdime.Entity;
using Ubik.Assets.Store.EF.Contracts;
using Ubik.Assets.Store.EF.POCO;
using Ubik.EF6;

namespace Ubik.Assets.Store.EF.Repositories
{
    public class MimeRepository : ReadWriteRepository<Mime, AssetsStoreDbContext>, IMimeRepository
    {
        public MimeRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }
    }
}