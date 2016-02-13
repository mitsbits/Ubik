using Ubik.Web.Components;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.BuildingBlocks
{
    public class CmsSlot : Slot
    {
        public CmsSlot(CmsSectionSlotInfo sectionSlotInfo, BasePartialModule module)
            : base(sectionSlotInfo, module)
        {
        }
    }
}