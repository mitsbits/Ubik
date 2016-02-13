using Microsoft.AspNet.Mvc.ViewFeatures;

namespace Ubik.Web.BuildingBlocks.Contracts
{
    internal interface IHtmlHelperRendersMe
    {
        void Render(HtmlHelper helper);
    }
}