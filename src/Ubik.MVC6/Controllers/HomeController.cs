using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Hosting;
using Ubik.Web.Components.Contracts;
using Ubik.Web.BuildingBlocks;
using Ubik.Web.Client.System.BuildingBlocks.Partials.Views;
using Ubik.Web.Components.Domain;
using Ubik.Web.Basis;

namespace Ubik.MVC6.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _env;
        public HomeController(IHostingEnvironment env)
        {
            _env = env;

        }
        public IActionResult Index()
        {
            ViewBag.ContentInfo = new PageContent() { Body = new[] { "first page", "second page"}, Title = "Ubik Index" };
            ViewBag.DeviceInfo = IndexDevice();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        private IDevice IndexDevice()
        {
            var result = new CmsDevice();
            result.Path = "Layouts/SingleColumn";
            result.Flavor = Web.Components.DeviceRenderFlavor.Empty;
            result.FriendlyName = "Single Column";
            result.Sections.Add(new CmsSection() { ForFlavor = Web.Components.DeviceRenderFlavor.Empty, Identifier = "html_head_section", FriendlyName = "Head Section" });

            var topNavSection = new CmsSection() { ForFlavor = Web.Components.DeviceRenderFlavor.Empty, Identifier = "top_nav_section", FriendlyName = "Top Nav" };
            topNavSection.Slots.Add(new CmsSlot(new CmsSectionSlotInfo("main_section", true, 1), new CmsPartialView("Top Nav", "~/Views/Shared/Partials/TopNav.cshtml")));
            result.Sections.Add(topNavSection);
            var mainSection = new CmsSection() { ForFlavor = Web.Components.DeviceRenderFlavor.Empty, Identifier = "main_section", FriendlyName = "Main Section" };
            mainSection.Slots.Add(new CmsSlot(new CmsSectionSlotInfo("main_section", true, 2), new PageBody("~/Views/Shared/Partials/System/PageBody.cshtml")));
            result.Sections.Add(mainSection);

            var footerSection = new CmsSection() { ForFlavor = Web.Components.DeviceRenderFlavor.Empty, Identifier = "footer_section", FriendlyName = "Footer Section" };
            footerSection.Slots.Add(new CmsSlot(new CmsSectionSlotInfo("footer_section", true,1), new CmsPartialView("Footer","~/Views/Shared/Partials/Footer.cshtml")));
            result.Sections.Add(footerSection);

            return result;
        }
    }
}
