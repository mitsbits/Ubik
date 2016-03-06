using Microsoft.AspNet.Mvc.ViewFeatures;
using System.Threading.Tasks;
using Ubik.Web.BuildingBlocks.Contracts;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.BuildingBlocks
{
    public class CmsPartialAction : PartialAction, IHtmlHelperRendersMe
    {
        public CmsPartialAction(string friendlyName, string action, string controller, string area)
            : base(friendlyName, action, controller, area)
        {
        }

        public virtual Task Render(HtmlHelper helper)
        {
            return Task.FromResult(0);
            //var dict = new ViewDataDictionary()
            //foreach (var value in RouteValues)
            //{
            //    dict.Add(value.Key, value.Value);
            //}

            //helper.RenderPartialAsync(Action, Controller,
        }
    }
}