using System.Collections.Generic;

namespace Ubik.Web.Components.Contracts
{
    internal interface IModule<out TData>
       where TData : IDictionary<string, object>
    {
        string FriendlyName { get; }

        TData Parameters { get; }

        ModuleType ModuleType { get; }
    }
}