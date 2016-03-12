using Mehdime.Entity;
using System;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using Ubik.Assets.Store.EF.Contracts;
using Ubik.Assets.Store.EF.POCO;
using Ubik.EF6;

namespace Ubik.Assets.Store.EF.Repositories
{
    public class AssetStoreProjectionRepository : ReadRepository<AssetStoreProjection, AssetsStoreDbContext>, IAssetStoreProjectionRepository
    {
        public AssetStoreProjectionRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }

        public async Task<Guid> Add(string filename, byte[] data, string parentFolder)
        {
            return await DbContext.AssetStoreAdd(parentFolder, filename, data);
        }
    }
}