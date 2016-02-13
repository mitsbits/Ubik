using System;
using System.Collections.Generic;
using System.Linq;
using Ubik.Web.Components.Contracts;
using Ubik.Web.Components.DTO;
using Ubik.Web.Components.Ext;

namespace Ubik.Web.Components.Domain
{

    public class PartialAction : BasePartialModule, IPartialAction
    {
        private const string KeyPrefix = "action::";
        private readonly string[] _keys = { "action", "controller", "area" };

        public PartialAction(string friendlyName, string action, string controller, string area)
            : base(friendlyName)
        {
            SetInternalValue(KeyPrefix, "action", action);
            SetInternalValue(KeyPrefix, "controller", controller);
            SetInternalValue(KeyPrefix, "area", area);
        }

        public PartialAction(string friendlyName, IDictionary<string, string> parameters)
            : base(friendlyName)
        {
            SetInternalValue(KeyPrefix, "action", parameters["action"]);
            SetInternalValue(KeyPrefix, "controller", parameters["controller"]);
            SetInternalValue(KeyPrefix, "area", parameters["area"]);

            var q = parameters
                .Where(x => !_keys.Contains(x.Key))
                .ToDictionary(x => string.Format("{0}{1}", KeyPrefix, x.Key), x => x.Value as object);
            Parameters.AppendAndUpdate(q);
        }

        public string Area
        {
            get { return GetInternalValue(KeyPrefix, "area").ToString(); }
            private set { SetInternalValue(KeyPrefix, "area", value); }
        }

        public string Controller
        {
            get { return GetInternalValue(KeyPrefix, "controller").ToString(); }
            private set { SetInternalValue(KeyPrefix, "controller", value); }
        }

        public string Action
        {
            get { return GetInternalValue(KeyPrefix, "action").ToString(); }
            private set { SetInternalValue(KeyPrefix, "action", value); }
        }

        public IReadOnlyDictionary<string, object> RouteValues
        {
            get
            {
                var result = Parameters.Cast<Tiding>().Where(x => !x.Key.StartsWith(KeyPrefix))
                        .ToDictionary(x => x.Key.Replace(KeyPrefix, string.Empty), x => x.Value as object);
                result.Add("area", Area);
                return result;
            }
        }

        public override ModuleType ModuleType
        {
            get { return ModuleType.PartialAction; }
        }
    }
}