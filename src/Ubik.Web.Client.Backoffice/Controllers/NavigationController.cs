using Microsoft.AspNet.Mvc;
using Ubik.Domain.Core;
using Ubik.Web.Basis.Contracts;
using Ubik.Web.BuildingBlocks.Contracts;

namespace Ubik.Web.Client.Backoffice.Controllers
{
    public class NavigationController : BackofficeController
    {
        private readonly IResident _resident;

        public NavigationController(IErrorLogManager errorLogManager, IEventBus eventDispatcher, IResident resident) : base(errorLogManager, eventDispatcher)
        {
            _resident = resident;
        }

        public ActionResult LeftMenu()
        {
            var model = _resident.Administration.BackofficeMenu;
            return PartialView(model);
        }
    }
}