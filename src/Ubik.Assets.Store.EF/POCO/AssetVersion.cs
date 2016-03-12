using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Assets.Store.EF.POCO
{
    public class AssetVersion
    {
        public Guid StreamId { get; set; }
        public int AssetId { get; set; }
        public int  Version { get; set; }
        public virtual Asset Asset { get; set; }
    }
}
