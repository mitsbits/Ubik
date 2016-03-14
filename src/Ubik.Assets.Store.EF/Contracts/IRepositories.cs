using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ubik.Assets.Store.EF.POCO;
using Ubik.EF6.Contracts;
using Ubik.Infra.Contracts;

namespace Ubik.Assets.Store.EF.Contracts
{
    public interface IAssetRepository : ICRUDRespoditory<Asset>, ISequenceRepository<Asset>
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

    public interface IAssetProjectionRepository : IReadRepository<AssetProjection>, IReadAsyncRepository<AssetProjection>
    {
        Task<IEnumerable<AssetProjection>> GetProjections(IEnumerable<int> ids);
    }
}