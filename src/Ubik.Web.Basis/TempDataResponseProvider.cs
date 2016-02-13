using Microsoft.AspNet.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Ubik.Infra.Contracts;
using Ubik.Infra.Ext;

namespace Ubik.Web.Basis
{
    public class TempDataResponseProvider : IServerResponseProvider
    {
        private readonly ICollection<IServerResponse> _bucket;
        internal const string Key = "796FF7DF-924D-4C78-80D8-C8149ACBBE4B";

        private TempDataResponseProvider(IEnumerable<IServerResponse> bucket)
        {
            _bucket = new HashSet<IServerResponse>(bucket);
        }

        public ICollection<IServerResponse> Messages
        {
            get { return _bucket; }
        }

        public static TempDataResponseProvider Create(ViewContext context)
        {
            return Create(context, Key);
        }

        private static TempDataResponseProvider Create(ViewContext context, string key)
        {
            if (context.TempData == null) return Empty();
            if (!context.TempData.ContainsKey(key)) return Empty();
            var bucket = context.TempData[key] as IEnumerable<string>;
            if (bucket == null || !bucket.Any()) return Empty();
            return new TempDataResponseProvider(bucket.Select(x=> x.FromJsonString<ServerResponse>()));
        }

        private static TempDataResponseProvider Empty()
        {
            return new TempDataResponseProvider(new List<IServerResponse>());
        }
    }
}