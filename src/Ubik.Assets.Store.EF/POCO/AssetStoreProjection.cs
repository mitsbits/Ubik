using System;

namespace Ubik.Assets.Store.EF.POCO
{
    public class AssetStoreProjection
    {
        public Guid stream_id { get; set; }
        public string full_path { get; set; }
        public string name { get; set; }
        public string file_type { get; set; }
        public DateTimeOffset creation_time { get; set; }
        public DateTimeOffset last_write_time { get; set; }
        public DateTimeOffset? last_access_time { get; set; }
        public bool is_directory { get; set; }
        public long? cached_file_size { get; set; }

    }
}