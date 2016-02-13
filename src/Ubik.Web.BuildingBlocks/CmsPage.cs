using Microsoft.AspNet.Mvc.Razor;

namespace Ubik.Web.BuildingBlocks
{
    public abstract class CmsPage<TModel> : RazorPage<TModel>
    {
        protected CmsPage()
        {
            Device = new DeviceHelper(ViewContext);
            Content = new ContentHelper(ViewContext);
        }

        public DeviceHelper Device { get; private set; }

        public ContentHelper Content { get; private set; }
    }

    public abstract class CmsPage : RazorPage
    {
        protected CmsPage()
        {
            Device = new DeviceHelper(ViewContext);
            Content = new ContentHelper(ViewContext);
        }

        public DeviceHelper Device { get; private set; }

        public ContentHelper Content { get; private set; }
    }
}