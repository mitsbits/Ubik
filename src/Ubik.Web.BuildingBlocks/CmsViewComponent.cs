using Ubik.Web.Components.Domain;
using Ubik.Web.Components.DTO;

namespace Ubik.Web.BuildingBlocks
{
    public class CmsViewComponent : PartialViewComponent
    {
        public CmsViewComponent(string friendlyName, string className, string typeFullName, Tidings invokeParameters)
            : base(friendlyName, className, typeFullName, invokeParameters)
        {
        }
    }
}