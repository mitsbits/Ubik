using Microsoft.AspNet.Mvc.Rendering;
using System.Collections.Generic;
using Ubik.Infra.Contracts;

namespace Ubik.Web.Basis.Helpers
{
    public class ServerResponseHelper : BasePageHelper, IServerResponseProvider
    {
        private readonly IServerResponseProvider _provider;

        public ServerResponseHelper(ViewContext viewContext) : base(viewContext)
        {
            _provider = TempDataResponseProvider.Create(viewContext);
        }

        public ICollection<IServerResponse> Messages => _provider.Messages;
    }
}