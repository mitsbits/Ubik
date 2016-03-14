using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.EF6.Contracts;

namespace Ubik.Assets.Store.EF.POCO
{
    public class Asset: ISequenceBase
    {
        public Asset()
        {
            Versions = new HashSet<AssetVersion>();
        }

        public int Id { get; set; }
        public int State { get; set; }
        public int CurrentVersion { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AssetVersion> Versions { get; set; }

    }
}
