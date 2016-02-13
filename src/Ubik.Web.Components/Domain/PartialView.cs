using System.Collections.Generic;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Components.Domain
{
    public class PartialView : BasePartialModule, IPartialView
    {
        private const string KeyPrefix = "view::";
        private readonly string[] _keys = { "path" };

        public PartialView(string friendlyName, string viewPath)
            : base(friendlyName)
        {
            SetInternalValue(KeyPrefix, "path", viewPath);
        }

        public PartialView(string friendlyName, string viewPath, IDictionary<string, object> parameters)
            : base(friendlyName, parameters)
        {
            SetInternalValue(KeyPrefix, "path", viewPath);
        }

        public string ViewPath
        {
            get { return GetInternalValue(KeyPrefix, "path").ToString(); }
        }

        public override ModuleType ModuleType
        {
            get { return ModuleType.PartialView; }
        }

        public PartialView SetViewPath(string newViewPath)
        {
            return new PartialView(FriendlyName, newViewPath, Parameters);
        }

        public PartialView SetParameter(string key, string value)
        {
            if (Parameters.ContainsKey(key))
            {
                Parameters[key] = value;
            }
            else { Parameters.Add(key, value); }
            return new PartialView(FriendlyName, ViewPath, Parameters);
        }

        public PartialView DeleteParameter(string key)
        {
            if (Parameters.ContainsKey(key))
            {
                Parameters.Remove(key);
            }
            return new PartialView(FriendlyName, ViewPath, Parameters);
        }
    }
}