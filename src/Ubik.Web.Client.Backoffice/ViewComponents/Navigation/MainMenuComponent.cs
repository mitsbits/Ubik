using Microsoft.AspNet.Mvc;
using Ubik.Web.BuildingBlocks.Contracts;

namespace Ubik.Web.Client.Backoffice.ViewComponents.Navigation
{
    [ViewComponent(Name = "Backoffice.Navigation.MainMenu")]
    public class MainMenuComponent : ViewComponent
    {
        private readonly IResident _resident;

        public MainMenuComponent(IResident resident)
        {
            _resident = resident;
        }

        public IViewComponentResult Invoke()
        {
            return View(_resident.Administration.BackofficeMenu);
        }
    }
}