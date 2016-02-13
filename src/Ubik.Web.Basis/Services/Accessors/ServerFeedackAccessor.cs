using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures.Internal;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Ubik.Infra.Contracts;
using Ubik.Web.Basis.Contracts;

namespace Ubik.Web.Basis.Services.Accessors
{
    public class ServerFeedackAccessor : IServerFeedbackAccessor, ICanHasViewContext
    {
        private ViewContext _viewContext;
        private IServerResponseProvider _provider;

        public ServerFeedackAccessor()
        {
        }

        public IReadOnlyCollection<IServerResponse> Current
        {
            get
            {
                if (_provider == null) _provider = TempDataResponseProvider.Create(_viewContext);
                return new ReadOnlyCollection<IServerResponse>(_provider.Messages.ToList());
            }
        }

        public void Contextualize(ViewContext viewContext)
        {
            _viewContext = viewContext;
        }
    }
}