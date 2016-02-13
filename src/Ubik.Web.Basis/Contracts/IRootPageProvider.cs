using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;

namespace Ubik.Web.Basis.Contracts
{
    internal interface IRootPageProvider
    {
        dynamic RootViewBag { get; }

        IDictionary<string, Object> RootRouteData { get; }

        ViewContext RootViewContext { get; }

        ITempDataDictionary RootTempData { get; }
    }
}