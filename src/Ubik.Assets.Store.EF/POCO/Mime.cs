using System.Collections.Generic;
using Ubik.EF6.Contracts;

namespace Ubik.Assets.Store.EF.POCO
{
    public class Mime : ISequenceBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
        public string DetailsTitle { get; set; }
        public string DetailsLink { get; set; }

    }
}