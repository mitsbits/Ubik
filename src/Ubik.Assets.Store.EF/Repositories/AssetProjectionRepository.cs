using Mehdime.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Assets.Store.EF.Contracts;
using Ubik.Assets.Store.EF.POCO;
using Ubik.EF6;

namespace Ubik.Assets.Store.EF.Repositories
{
    public class AssetProjectionRepository : ReadRepository<AssetProjection, AssetsStoreDbContext>, IAssetProjectionRepository
    {
        public AssetProjectionRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }

        public async Task<IEnumerable<AssetProjection>> GetProjections(IEnumerable<int> ids)
        {
            return await DbContext.AssetProjections.Where(x => ids.Contains(x.Id)).ToListAsync();
        }
    }
}