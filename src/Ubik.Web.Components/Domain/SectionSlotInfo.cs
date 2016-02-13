using System;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Components.Domain
{

    public class SectionSlotInfo : ISectionSlotInfo
    {
        private SectionSlotInfo()
        {
        }

        public SectionSlotInfo(string sectionIdentifier, bool enabled, int ordinal)
            : this()
        {
            Guard.ForNullOrEmpty(sectionIdentifier, "sectionIdentifier");
            SectionIdentifier = sectionIdentifier;
            Enabled = enabled;
            Ordinal = ordinal;
        }

        public string SectionIdentifier { get; private set; }

        public bool Enabled { get; private set; }

        public int Ordinal { get; private set; }

        public SectionSlotInfo SetEnabled(bool state)
        {
            return state == Enabled ? this : new SectionSlotInfo(SectionIdentifier, state, Ordinal);
        }

        public SectionSlotInfo SetIndex(int index)
        {
            return index == Ordinal ? this : new SectionSlotInfo(SectionIdentifier, Enabled, index);
        }
    }
}