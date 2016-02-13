using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Web.Client.Backoffice.ViewComponents.Navigation
{
    [ViewComponent(Name = "Backoffice.Navigation.Breadcrump")]
    public class BreadcrumpComponent : ViewComponent
    {
        public BreadcrumpComponent() { }

        public  IViewComponentResult Invoke()
        {
            
            return View();
        }
    }
}
