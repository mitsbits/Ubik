using System;
using System.Threading.Tasks;
using Mehdime.Entity;
using Ubik.Assets.Store.EF.Contracts;
using Ubik.Assets.Store.EF.POCO;
using Ubik.EF6;
using Ubik.EF6.Contracts;

namespace Ubik.Assets.Store.EF.Repositories
{
    public class AssetRepository : ReadWriteRepository<Asset, AssetsStoreDbContext>, IAssetRepository
    {
        public AssetRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }

        public async Task<int> GetNext()
        {
            var sepProvider = DbContext as ISequenceProvider;
            if (sepProvider == null)
                throw new ApplicationException(string.Format("{0} does not implement {1}",
                    nameof(DbContext), typeof(ISequenceProvider).AssemblyQualifiedName));
            return await sepProvider.GetNext(typeof(Asset));
        }
    }
}