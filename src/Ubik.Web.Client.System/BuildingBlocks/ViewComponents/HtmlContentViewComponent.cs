using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Ubik.Web.Components;
using Ubik.Web.Components.Domain;
using Ubik.Web.Components.Contracts;
using Ubik.Web.Components.DTO;

namespace Ubik.Web.Client.System.BuildingBlocks.ViewComponents
{
    [ViewComponent(Name = "Ubik.Web.Client.System.BuildingBlocks.ViewComponents.HtmlContentViewComponent")]
    public class HtmlContentViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string id, string view = "")
        {
            return View(view, id);
        }

    }

    public class HtmlContentViewComponentDescriptor : PartialViewComponent, IModuleDescriptor
    {
        public HtmlContentViewComponentDescriptor(string friendlyName, IEnumerable<Tiding> invokeParameters)
            : base(friendlyName, typeof(HtmlContentViewComponent).Name, typeof(HtmlContentViewComponent).AssemblyQualifiedName, invokeParameters)
        {

        }

        public HtmlContentViewComponentDescriptor() : this("Html Content",new Tidings(new Tiding[] { new Tiding() { Key = "id", Value = "1" }, new Tiding() { Key = "view", Value = "~/Views/Shared/Partials/System/HtmlContent.cshtml" } }) )
        {

        }

        public string ModuleGroup
        {
            get
            {

                return "System - Content";
            }
        }

        public override ModuleType ModuleType
        {
            get
            {
                return ModuleType.ViewComponent;
            }
        }

        public string Summary
        {
            get
            {
                return "A components that renders Html content based on name or id.";
            }
        }

        public BasePartialModule Default()
        {


            var result =  new HtmlContentViewComponentDescriptor();
            return result;
        }
    }
}
