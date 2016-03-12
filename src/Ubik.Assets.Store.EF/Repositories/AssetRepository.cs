using Mehdime.Entity;
using Ubik.Assets.Store.EF.Contracts;
using Ubik.Assets.Store.EF.POCO;
using Ubik.EF6;

namespace Ubik.Assets.Store.EF.Repositories
{
    public class AssetRepository : ReadWriteRepository<Asset, AssetsStoreDbContext>, IAssetRepository
    {
        public AssetRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }  
    }
}