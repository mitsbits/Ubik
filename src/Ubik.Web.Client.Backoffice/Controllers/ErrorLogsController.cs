using Microsoft.AspNet.Mvc;
using System;
using System.Threading.Tasks;
using Ubik.Domain.Core;
using Ubik.Infra;
using Ubik.Web.Basis.Contracts;
using Ubik.Web.Client.Backoffice.ViewModel;

namespace Ubik.Web.Client.Backoffice.Controllers
{
    public class ErrorLogsController : BackofficeController
    {
        private readonly IErrorLogManager _manager;

        public ErrorLogsController(IErrorLogManager manager, IEventBus eventDispatcher) : base(manager, eventDispatcher)
        {
            _manager = manager;
        }

        public async Task<ActionResult> Index()
        {
            var model = await _manager.GetLogs(Pager.Current, Pager.RowCount);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteErrorLog(DeleteErrorLogViewModel model)
        {
            await _manager.ClearLog(model.ErrorId.ToString());
            return Redirect(model.RedirectUrl);
        }

        [HttpPost]
        public async Task<ActionResult> ClearLogsForRange(DeleteErrorLogRangeViewModel model)
        {
            try
            {
                var rowsDeleted = await _manager.ClearLogs(model.RangeStart, model.RangeEnd);
                AddRedirectMessage(ServerResponseStatus.SUCCESS, string.Format("{0} logs deleted!", rowsDeleted));
                return Redirect(model.RedirectUrl);
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
                return RedirectToAction("Index", "ErrorLogs", null);
            }
        }
    }
}