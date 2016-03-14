namespace Ubik.Assets.Store.Core.Contracts
{
    public interface IAssetInfo<out TKey>: IAssetInfo
    {
        TKey Id { get; }

    }

    public interface IAssetInfo
    {
        AssetState State { get; }
        IVersionInfo CurrentFile { get; }
        string Name { get; }
    }
}