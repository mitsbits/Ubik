using System;
using System.Threading.Tasks;
using Ubik.Assets.Store.EF.POCO;
using Ubik.Infra.Contracts;

namespace Ubik.Assets.Store.EF.Contracts
{
    public interface IAssetRepository : ICRUDRespoditory<Asset>
    {
    }

    public interface IAssetVersionRepository : ICRUDRespoditory<AssetVersion>
    {
    }

    public interface IMimeRepository : ICRUDRespoditory<Mime>
    {
    }

    public interface IAssetStoreProjectionRepository : IReadRepository<AssetStoreProjection>, IReadAsyncRepository<AssetStoreProjection>
    {
        Task<bool> Exists(string filename, string parentFolder);
        Task<Guid> Add(string filename, byte[] data, string parentFolder);
    }
}