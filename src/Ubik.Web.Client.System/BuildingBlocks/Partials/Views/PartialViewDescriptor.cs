
using Ubik.Web.Components;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.Client.System.BuildingBlocks.Partials.Views
{
    public class PartialViewDescriptor : BaseModuleDescriptor
    {
        public override string FriendlyName
        {
            get
            {
                return "Partial View";
            }
        }

        public override ModuleType ModuleType
        {
            get
            {
                return ModuleType.PartialView;
            }
        }

        public override string Summary
        {
            get
            {
                return "Renders the Razor view from the path specified.";
            }
        }

        public override BasePartialModule Default()
        {
            return new PartialView(string.Empty, string.Empty);
        }
    }
}
