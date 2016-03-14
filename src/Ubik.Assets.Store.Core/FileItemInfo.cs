using System;
using Ubik.Assets.Store.Core.Contracts;

namespace Ubik.Assets.Store.Core
{
    public class FileItemInfo<TKey> : IFileInfo<TKey>
    {
        public TKey Id { get; set; }
        public string FullPath { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastWrite { get; set; }
        public DateTime? LastRead { get; set; }
        public bool IsDirectory { get; set; }
        public long FileSize { get; set; }
        public string MimeType { get; set; }
    }




}