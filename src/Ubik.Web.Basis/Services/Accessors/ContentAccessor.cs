using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures.Internal;
using Ubik.Web.Basis.Contracts;

namespace Ubik.Web.Basis.Services.Accessors
{
    public class ContentAccessor : IContentAccessor, ICanHasViewContext
    {
        private ViewContext _viewContext;

        public ContentAccessor()
        {
        }

        private IPageContent _current;

        public IPageContent Current
        {
            get
            {
                var page = _current ?? (_current = (_viewContext.ViewBag.ContentInfo as IPageContent));
                if (page != null) return page;
                page = Default();
                _viewContext.ViewBag.ContentInfo = page;
                return page;
            }
        }

        private IPageContent Default()
        {
            var page = new PageContent() { Title = "Ubik 1.0" };

            return page;
        }

        public void Contextualize(ViewContext viewContext)
        {
            _viewContext = viewContext;
        }
    }
}