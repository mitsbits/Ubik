using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Assets.Store.Core
{
    public class AssetInfo<TKey>
    {
        public TKey StreamId { get; set; }
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
