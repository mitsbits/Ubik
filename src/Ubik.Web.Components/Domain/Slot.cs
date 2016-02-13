using System;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Components.Domain
{

    public class Slot : ISlot
    {
        public Slot(SectionSlotInfo sectionSlotInfo, BasePartialModule module)
        {
            SectionSlotInfo = sectionSlotInfo;
            Module = module;
        }

        ISectionSlotInfo ISlot.SectionSlotInfo { get { return SectionSlotInfo; } }

        public SectionSlotInfo SectionSlotInfo { get; private set; }
        public BasePartialModule Module { get; private set; }

        public Slot ReplaceModule(BasePartialModule module)
        {
            return new Slot(SectionSlotInfo, module);
        }

        public Slot ReplaceSlotInfo(SectionSlotInfo sectionSlotInfo)
        {
            return new Slot(sectionSlotInfo, Module);
        }
    }
}