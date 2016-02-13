using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using Ubik.Infra.Contracts;
using Ubik.Web.Basis;
using System.Linq;
using Ubik.Infra.Ext;

namespace Ubik.Web
{
    public static class TempDataResponseProviderExtentions
    {
        public static void AddRedirectMessages(this Controller controller, params ServerResponse[] messages)
        {
            var source = controller.TempData[TempDataResponseProvider.Key] as IEnumerable<IServerResponse>;
            var bucket = source == null ? new List<string>() : new List<string>(source.Select(x => x.ToJsonString()));
            bucket.AddRange(messages.Select(x=> x.ToJsonString()));
            controller.TempData[TempDataResponseProvider.Key] = bucket;
        }
    }
}