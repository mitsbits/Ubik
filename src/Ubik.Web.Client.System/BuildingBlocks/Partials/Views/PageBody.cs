using System.Collections.Generic;
using Ubik.Web.Components;
using Ubik.Web.Components.Contracts;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.Client.System.BuildingBlocks.Partials.Views
{
    public class PageBody : PartialView, IModuleDescriptor
    {
        private static PageBody _defaultInstance;

        public PageBody(string viewPath = "") : this("Page Body", viewPath)
        {
        }

        internal PageBody(string friendlyName, string viewPath)
            : base(friendlyName, viewPath)
        {
        }

        internal PageBody(string friendlyName, string viewPath, IDictionary<string, object> parameters)
            : base(friendlyName, viewPath, parameters)
        {
        }

        public string Summary
        {
            get { return @"A partial view for displaying the basic content of a page."; }
        }

        public string ModuleGroup
        {
            get { return "System"; }
        }

        public BasePartialModule Default()
        {
            if (_defaultInstance != null) return _defaultInstance;
            _defaultInstance = new PageBody();
            return _defaultInstance;
        }
    }
}