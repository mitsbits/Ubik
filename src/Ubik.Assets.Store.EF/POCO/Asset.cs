using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Assets.Store.EF.POCO
{
    public class Asset
    {
        public int Id { get; set; }
        public int State { get; set; }
        public int CurrentVersion { get; set; }
        public string Name { get; set; }

        public int MimeId { get; set; }

        public virtual Mime Mime { get; set; }
    }
}
