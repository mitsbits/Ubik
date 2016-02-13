using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Ubik.Web.BuildingBlocks.Contracts;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.BuildingBlocks
{
    public class CmsPartialView : PartialView, IHtmlHelperRendersMe
    {
        public CmsPartialView(string friendlyName, string viewPath)
            : base(friendlyName, viewPath)
        {
        }

        public virtual void Render(HtmlHelper helper)
        {
            helper.RenderPartialAsync(ViewPath, Parameters);
        }
    }
}