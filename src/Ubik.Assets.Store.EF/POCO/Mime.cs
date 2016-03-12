using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Assets.Store.EF.POCO
{
    public class Mime
    {
        public Mime()
        {
            
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
        public string DetailsTitle { get; set; }
        public string DetailsLink { get; set; }
    }
}
