using Microsoft.AspNet.Mvc.Rendering;
using Ubik.Web.Basis;
using Ubik.Web.BuildingBlocks.Contracts;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.BuildingBlocks
{
    public class ContentHelper : BasePageHelper, IContentPageProvider
    {
        public ContentHelper(ViewContext viewContext)
            : base(viewContext)
        {
        }

        private IContent _content;

        public IContent Current => _content ?? (_content = RootViewBag.Content as IContent);
    }
}