using Mehdime.Entity;
using System;
using System.Threading.Tasks;
using Ubik.Assets.Store.EF.Contracts;
using Ubik.Assets.Store.EF.POCO;
using Ubik.EF6;
using System.Linq;
using System.IO;

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

        public Task<bool> Exists(string filename, string parentFolder)
        {
            //TODO: remove the call from db context
            var pTbl = AssetsStoreDbContext.GetParentDirectories(parentFolder);

            var dirs = pTbl.Select().OrderBy(x => x[1]).Select(x => x[0]);

            var expected = Path.Combine(
                (new[] { DbContext.AssetStoreRoot })
                .Union(dirs.Select(x => x.ToString()))
                .Union(new[] { filename })
                .ToArray());


            var hit = DbContext.Projections.Where(x => x.full_path == expected).FirstOrDefault();

            return Task.FromResult(hit != null);

        }
    }
}