﻿using System.Threading.Tasks;

namespace Ubik.Assets.Store.Core.Contracts
{
    public interface IStoreService<TKey>
    {
        Task<IFileInfo<TKey>> Upload(byte[] content, string fileName, string parentFolder);


    }

    public interface IAssetService<TKey>
    {
        Task<IAssetInfo<TKey>> Create(string name, AssetState state, byte[] content, string fileName);
        Task Suspend(TKey id);
        Task Acivate(TKey id);
        Task<IAssetInfo<TKey>> AddNewVersion(TKey id, byte[] content, string fileName);
    }
}