using Microsoft.AspNet.Mvc.ViewFeatures;
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

        public virtual void Render(HtmlHelper helper)
        {
            //var dict = new ViewDataDictionary()
            //foreach (var value in RouteValues)
            //{
            //    dict.Add(value.Key, value.Value);
            //}

            //helper.RenderPartialAsync(Action, Controller,
        }
    }
}