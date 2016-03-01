using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;
using System;
using System.Linq;
using Ubik.Domain.Core;
using Ubik.Infra;
using Ubik.Web.Basis;
using Ubik.Web.Basis.Contracts;
using Ubik.Web.Client.Backoffice.Contracts;
using System.Threading.Tasks;
using System.Diagnostics;
using Ubik.Web.Membership.Events;

namespace Ubik.Web.Client.Backoffice.Controllers
{
    //[Authorize(Policy = "Over18")]
    [Area("Backoffice")]
    public abstract class BackofficeController : Controller
    {
        private readonly IErrorLogManager _errorLogManager;
        protected readonly IEventBus _eventDispatcher;

        protected BackofficeController(IErrorLogManager errorLogManager, IEventBus eventDispatcher)
        {
            _errorLogManager = errorLogManager;
            _eventDispatcher = eventDispatcher;
        }

        #region Pager

        private const string pageNumerVariableName = "p";
        private const string rowCountVariableName = "r";

        protected RequestPager Pager
        {
            get
            {
                var p = 1;

                if (!string.IsNullOrWhiteSpace(Request.Query[pageNumerVariableName]))
                    int.TryParse(Request.Query[pageNumerVariableName], out p);

                var r = 10;

                if (!string.IsNullOrWhiteSpace(Request.Query[rowCountVariableName]))
                    int.TryParse(Request.Query[rowCountVariableName], out r);
                return new RequestPager() { Current = p, RowCount = r };
            }
        }

        protected struct RequestPager
        {
            public int Current { get; set; }
            public int RowCount { get; set; }
        }

        #endregion Pager

        protected void SetContentPage(IBackofficeContent content)
        {
            var viewBag = GetRootViewBag();
            viewBag.ContentInfo = content;
           // _eventDispatcher.Publish(new ContentSetEvent() { Title = content.Title });
           // _eventDispatcher.Publish(new RolePersisted());
        }

        private dynamic GetRootViewBag()
        {
            return ViewBag;
        }

        #region Redirect Messages

        protected void AddRedirectMessage(ServerResponseStatus status, string title, string message = "")
        {
            this.AddRedirectMessages(new[] { new ServerResponse(status, title, message) });
        }

        protected void AddRedirectMessage(Exception exception)
        {
            _errorLogManager.LogException(exception);
            this.AddRedirectMessages(new[] { new ServerResponse(exception) });
        }

        protected void AddRedirectMessage(ModelStateDictionary state)
        {
            foreach (var stm in state.Where(stm => stm.Value.Errors != null))
            {
                this.AddRedirectMessages(
                    stm.Value.Errors.Select(
                        e => new ServerResponse(ServerResponseStatus.ERROR, e.ErrorMessage, e.Exception.Message))
                        .ToArray());
            }
        }

        #endregion Redirect Messages


    }
}