using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using System.Threading.Tasks;
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

        public virtual async Task Render(HtmlHelper helper)
        {
           await helper.RenderPartialAsync(ViewPath, Parameters);
        }
    }
}