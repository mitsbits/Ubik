namespace Ubik.Assets.Store.Core.Contracts
{
    public interface IVersionInfo
    {
        int Version { get; }

        IFileInfo FileInfo { get; }
    }
}