using System;
using System.Collections.Generic;
using System.Linq;
using Ubik.Web.Components.Contracts;
using Ubik.Web.Components.DTO;
using Ubik.Web.Components.Ext;

namespace Ubik.Web.Components.Domain
{
    public class PartialViewComponent : BasePartialModule, IPartialViewComponent
    {
        private const string KeyPrefix = "viewcomponent::";
        private readonly string[] _keys = { "className", "typeFullName" };

        public PartialViewComponent(string friendlyName, string className, string typeFullName, IEnumerable<Tiding> invokeParameters)
            : base(friendlyName)
        {
            ClassName = className;
            TypeFullName = typeFullName;
            foreach (var invokeParameter in invokeParameters)
            {
                var key = string.Format("{0}{1}", KeyPrefix, invokeParameter.Key);
                if (Parameters.ContainsKey(key))
                {
                    var hit = Parameters.Cast<Tiding>().FirstOrDefault(x => x.Key == key);
                    hit.Weight = invokeParameter.Weight;
                    hit.Value = invokeParameter.Value;
                    hit.Hint = invokeParameter.Hint;
                }
                else
                {
                    Parameters.Add(new Tiding() { Key = key, Hint = invokeParameter.Hint, Value = invokeParameter.Value });
                }
            }
        }

        public virtual string ClassName
        {
            get { return GetInternalValue(KeyPrefix, "className").ToString(); }
            private set { SetInternalValue(KeyPrefix, "className", value); }
        }

        public virtual string TypeFullName
        {
            get { return GetInternalValue(KeyPrefix, "typeFullName").ToString(); }
            private set { SetInternalValue(KeyPrefix, "typeFullName", value); }
        }

        public virtual Object[] InvokeParameters
        {
            get
            {
                return
                    Parameters.Cast<Tiding>()
                    .Where(x => x.Key.StartsWith(KeyPrefix)
                     && !_keys.Contains(x.Key.Replace(KeyPrefix, string.Empty)))
                        .ToList().ValuesFromHint();
            }
        }

        public override ModuleType ModuleType
        {
            get { return ModuleType.ViewComponent; }
        }
    }
}