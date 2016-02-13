using System.Collections.Generic;

namespace Ubik.Web.Components.Contracts
{
    internal interface IPartialAction
    {
        string Area { get; }

        string Controller { get; }

        string Action { get; }

        IReadOnlyDictionary<string, object> RouteValues { get; }
    }
}