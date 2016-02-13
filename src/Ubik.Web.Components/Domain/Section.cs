using System;
using System.Collections.Generic;
using System.Linq;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Components.Domain
{

    public class Section<TKey> : EntityBase<TKey>, ISection
    {
        private Section()
        {
            Slots = new HashSet<Slot>();
        }

        internal Section(TKey id, string identifier, string friendlyName)
            : this(id, identifier, friendlyName, DeviceRenderFlavor.Empty)
        {
        }

        public Section(TKey id, string identifier, string friendlyName, DeviceRenderFlavor flavor)
            : this()
        {
            Id = id;
            Identifier = identifier;
            FriendlyName = friendlyName;
            _forFlavor = flavor;
        }

        public string Identifier { get; private set; }

        public string FriendlyName { get; private set; }

        ICollection<ISlot> ISection.Slots { get { return Slots.Cast<ISlot>().ToList(); } }
        public ICollection<Slot> Slots { get; private set; }
        private DeviceRenderFlavor _forFlavor = DeviceRenderFlavor.Empty;

        public DeviceRenderFlavor ForFlavor
        {
            get { return _forFlavor; }
        }

        public void DefineSlot(SectionSlotInfo info, BasePartialModule module)
        {
            var slot = Slots
                .FirstOrDefault(x => x.SectionSlotInfo.Ordinal == info.Ordinal
                    && x.SectionSlotInfo.SectionIdentifier == info.SectionIdentifier);

            if (slot != null) Slots.Remove(slot);

            Slots.Add(new Slot(info, module));
        }

        public void SetForFlavor(DeviceRenderFlavor newFlavor)
        {
            _forFlavor = newFlavor;
        }
    }
}