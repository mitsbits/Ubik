using Ubik.Web.Components.Domain;

namespace Ubik.Web.BuildingBlocks
{
    public class CmsSectionSlotInfo : SectionSlotInfo
    {
        public CmsSectionSlotInfo(string sectionIdentifier, bool enabled, int ordinal)
            : base(sectionIdentifier, enabled, ordinal)
        {
        }
    }
}