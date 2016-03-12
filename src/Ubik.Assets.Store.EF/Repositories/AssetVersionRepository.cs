using Mehdime.Entity;
using Ubik.Assets.Store.EF.Contracts;
using Ubik.Assets.Store.EF.POCO;
using Ubik.EF6;

namespace Ubik.Assets.Store.EF.Repositories
{
    public class AssetVersionRepository : ReadWriteRepository<AssetVersion, AssetsStoreDbContext>, IAssetVersionRepository
    {
        public AssetVersionRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }
    }
}