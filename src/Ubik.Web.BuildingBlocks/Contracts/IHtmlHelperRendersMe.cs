using Microsoft.AspNet.Mvc.ViewFeatures;
using System.Threading.Tasks;

namespace Ubik.Web.BuildingBlocks.Contracts
{
    internal interface IHtmlHelperRendersMe
    {
        Task Render(HtmlHelper helper);
    }
}