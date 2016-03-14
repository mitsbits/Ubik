using Ubik.Assets.Store.Core.Contracts;

namespace Ubik.Assets.Store.Core
{
    public class VersionItemInfo : IVersionInfo
    {
        public IFileInfo FileInfo { get; set; }
        public int Version { get; set; }
    }
}