//using Microsoft.AspNet.Mvc.Razor;
//using Microsoft.AspNet.Mvc.ViewEngines;

//namespace Ubik.Web.BuildingBlocks
//{
//    public class CmsViewEngine : RazorViewEngine
//    {
//        public CmsViewEngine()
//            : base()
//        {
//            AreaViewLocationFormats = new string[6]
//                  {
//                    "~/Areas/{2}/Views/_plugins/%1/{1}/{0}.cshtml",
//                    "~/Areas/{2}/Views/_plugins/%1/{1}/{0}.vbhtml",
//                    "~/Areas/{2}/Views/{1}/{0}.cshtml",
//                    "~/Areas/{2}/Views/{1}/{0}.vbhtml",
//                    "~/Areas/{2}/Views/Shared/{0}.cshtml",
//                    "~/Areas/{2}/Views/Shared/{0}.vbhtml"
//                  };
//            AreaMasterLocationFormats = new string[6]
//                  {
//                    "~/Areas/{2}/Views/_plugins/%1/{1}/{0}.cshtml",
//                    "~/Areas/{2}/Views/_plugins/%1/{1}/{0}.vbhtml",
//                    "~/Areas/{2}/Views/{1}/{0}.cshtml",
//                    "~/Areas/{2}/Views/{1}/{0}.vbhtml",
//                    "~/Areas/{2}/Views/Shared/{0}.cshtml",
//                    "~/Areas/{2}/Views/Shared/{0}.vbhtml"
//                  };
//            AreaPartialViewLocationFormats = new string[6]
//                  {
//                    "~/Areas/{2}/Views/_plugins/%1/{1}/{0}.cshtml",
//                    "~/Areas/{2}/Views/_plugins/%1/{1}/{0}.vbhtml",
//                    "~/Areas/{2}/Views/{1}/{0}.cshtml",
//                    "~/Areas/{2}/Views/{1}/{0}.vbhtml",
//                    "~/Areas/{2}/Views/Shared/{0}.cshtml",
//                    "~/Areas/{2}/Views/Shared/{0}.vbhtml"
//                  };
//        }

//        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
//        {
//            if (controllerContext.RouteData.Values["Plugin"] != null)
//            {
//                var pluginName = controllerContext.RouteData.Values["Plugin"].ToString();
//                return base.CreatePartialView(controllerContext, partialPath.Replace("%1", pluginName));
//            }
//            return base.CreatePartialView(controllerContext, partialPath);
//        }

//        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
//        {
//            if (controllerContext.RouteData.Values["Plugin"] != null)
//            {
//                var pluginName = controllerContext.RouteData.Values["Plugin"].ToString();
//                return base.CreateView(controllerContext, viewPath.Replace("%1", pluginName), masterPath);
//            }
//            return base.CreateView(controllerContext, viewPath, masterPath);
//        }

//        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
//        {
//            if (controllerContext.RouteData.Values["Plugin"] != null)
//            {
//                var pluginName = controllerContext.RouteData.Values["Plugin"].ToString();
//                return base.FileExists(controllerContext, virtualPath.Replace("%1", pluginName));
//            }
//            return base.FileExists(controllerContext, virtualPath);
//        }
//    }
//}