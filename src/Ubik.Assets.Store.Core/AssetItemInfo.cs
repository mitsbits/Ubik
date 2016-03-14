using Ubik.Assets.Store.Core.Contracts;

namespace Ubik.Assets.Store.Core
{
    public class AssetItemInfo<TKey> : IAssetInfo<TKey>
    {
        public IVersionInfo CurrentFile { get; set; }

        public TKey Id { get; set; }

        public string Name { get; set; }

        public AssetState State { get; set; }
    }
}