using System.Collections.Generic;
using Ubik.Web.Components;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.BuildingBlocks
{
    public class CmsSection : ISection
    {
        private readonly ICollection<ISlot> _slots;

        public CmsSection()
        {
            _slots = new HashSet<ISlot>();
        }

        public string Identifier { get; set; }

        public string FriendlyName { get; set; }

        public ICollection<ISlot> Slots
        {
            get { return _slots; }
        }

        public DeviceRenderFlavor ForFlavor { get; set; }
    }
}