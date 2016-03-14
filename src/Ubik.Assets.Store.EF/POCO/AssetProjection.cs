using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Assets.Store.EF.POCO
{
    public class AssetProjection
    {
        public int Id { get; set; }
        public int State { get; set; }
        public int CurrentVersion { get; set; }
        public string Name { get; set; }
        public string MimeName { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
        public Guid stream_id { get; set; }
        public string full_path { get; set; }
        public string FileName { get; set; }
        public string file_type { get; set; }
        public DateTimeOffset creation_time { get; set; }
        public DateTimeOffset last_write_time { get; set; }
        public DateTimeOffset? last_access_time { get; set; }
        public bool is_directory { get; set; }
        public long? cached_file_size { get; set; }
    }
}
