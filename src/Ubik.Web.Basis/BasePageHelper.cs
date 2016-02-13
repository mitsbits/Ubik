using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Linq;
using Ubik.Web.Basis.Contracts;

namespace Ubik.Web.Basis
{
    public abstract class BasePageHelper : IRootPageProvider
    {
        protected readonly ViewContext _rootViewContext;
        protected readonly ITempDataDictionary _rootTempData;
        private dynamic _viewBag;
        private IDictionary<string, object> _routeData;

        protected BasePageHelper(ViewContext viewContext)
        {
            //_rootViewContext = viewContext;
            //while (_rootViewContext.IsChildAction)
            //{
            //    _rootViewContext = _rootViewContext.ParentActionViewContext;
            //}
            //_rootTempData = _rootViewContext.TempData;
            _rootViewContext = viewContext;
            _rootTempData = _rootViewContext.TempData;
        }

        public virtual dynamic RootViewBag => _viewBag ?? (_viewBag = GetPageViewBag());

        public virtual IDictionary<string, object> RootRouteData => _routeData ?? (_routeData = GetPageRouteData());

        private dynamic GetPageViewBag()
        {
            return RootViewContext.ViewBag;
        }

        private Dictionary<string, object> GetPageRouteData()
        {
            return RootViewContext.RouteData.Values.ToDictionary(x => x.Key, x => x.Value);
        }

        public ViewContext RootViewContext => _rootViewContext;

        public ITempDataDictionary RootTempData => RootViewContext.TempData;
    }
}