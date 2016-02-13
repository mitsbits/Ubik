using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures.Internal;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.BuildingBlocks.Contracts
{
    public interface IDeviceAccesor
    {
        IDevice Current { get; }
    }

    public class DeviceAccessor : IDeviceAccesor, ICanHasViewContext
    {
        private ViewContext _viewContext;

        public DeviceAccessor()
        {
        }

        private IDevice _current;

        public IDevice Current
        {
            get
            {
                var page = _current ?? (_current = (_viewContext.ViewBag.ContentInfo as IDevice));
                if (page != null) return page;
                page = Default();
                _viewContext.ViewBag.DeviceInfo = page;
                return page;
            }
        }

        private IDevice Default()
        {
            var page = new CmsDevice()
            {
                Flavor = Components.DeviceRenderFlavor.Empty,
                FriendlyName = "Default",
                Path = "_Layout"
            };

            return page;
        }

        public void Contextualize(ViewContext viewContext)
        {
            _viewContext = viewContext;
        }
    }
}