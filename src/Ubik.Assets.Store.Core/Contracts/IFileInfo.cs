using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Assets.Store.Core.Contracts
{
    public interface IFileInfo<out TKey> : IFileInfo
    {
        TKey Id { get; }

    }

    public interface IFileInfo
    {
        string FullPath { get; }
        string Name { get; }
        string FileType { get; }
        DateTime CreationDate { get; }
        DateTime LastWrite { get; }
        DateTime? LastRead { get; }
        bool IsDirectory { get; }
        long FileSize { get; }
        string MimeType { get; }
    }


}
