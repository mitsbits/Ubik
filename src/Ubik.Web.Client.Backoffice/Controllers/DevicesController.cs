using Microsoft.AspNet.Mvc;
using System;
using System.Threading.Tasks;
using Ubik.Domain.Core;
using Ubik.Infra;
using Ubik.Web.Basis.Contracts;
using Ubik.Web.Components.AntiCorruption.Contracts;
using Ubik.Web.Components.AntiCorruption.ViewModels.Devices;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Client.Backoffice.Controllers
{
    public class DevicesController : BackofficeController
    {
        private readonly IDeviceAdministrationService<int> _deviceService;
        private readonly IDeviceAdministrationViewModelService _deviceViewModels;

        public DevicesController(IErrorLogManager errorLogManager, IEventBus eventDispatcher, IDeviceAdministrationService<int> deviceService, IDeviceAdministrationViewModelService deviceViewModels) : base(errorLogManager, eventDispatcher)
        {
            _deviceService = deviceService;
            _deviceViewModels = deviceViewModels;
        }

        public Task<ActionResult> Layouts(int? id, int? sectionId)
        {
            if (!id.HasValue) return AllDevices();

            return id.Value > default(int) ? OneDeviceById(id.Value, (sectionId??0)) : NewDevice();
        }

        private async Task<ActionResult> NewDevice()
        {
            SetContentPage(new BackofficeContent() { Title = "Layouts", Subtitle = "create a template for pages" });
            var model = await _deviceViewModels.DeviceModel(0);
            model.SelectedSectionId = default(int);
            return View(model);
        }

        private async Task<ActionResult> OneDeviceById(int value, int sectionId)
        {
            var model = await _deviceViewModels.DeviceModel(value);
            model.SelectedSectionId = sectionId;
            SetContentPage(new BackofficeContent() { Title = string.Format( "Layouts: {0}", model.FriendlyName), Subtitle = (model.SelectedSection != null)? string.Format("Section: {0}", model.SelectedSection.FriendlyName): string.Empty });
            return View(model);
        }

        private async Task<ActionResult> AllDevices()
        {
            SetContentPage(new BackofficeContent() { Title = "Layouts", Subtitle = "template for pages" });
            return View(await _deviceViewModels.DeviceModels());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateLayout(DeviceViewModel model)
        {
            try
            {
                var isNew = model.Id == default(int);
                if (!ModelState.IsValid)
                {
                    AddRedirectMessage(ModelState);
                    return View("Layouts", model);
                }
                await _deviceViewModels.Execute(model);
                AddRedirectMessage(ServerResponseStatus.SUCCESS, string.Format("Device '{0}' {1}!", model.FriendlyName, (isNew) ? "created" : "updated"));
                return RedirectToAction("layouts", "devices", new { id = model.Id });
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
                return RedirectToAction("layouts", "devices", null);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateSection(SectionSaveModel model)
        {
            try
            {
                var isNew = model.SectionId == default(int);
                if (!ModelState.IsValid)
                {
                    AddRedirectMessage(ModelState);
                    return RedirectToAction("LayOuts", "Devices", new { id = model.DeviceId });
                }
                await _deviceViewModels.Execute(model);
                AddRedirectMessage(ServerResponseStatus.SUCCESS, string.Format("Section '{0}' {1}!", model.FriendlyName, (isNew) ? "created" : "updated"));
                return RedirectToAction("Layouts", "Devices", new { id = model.DeviceId, section = model.SectionId });
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
                return RedirectToAction("Layouts", "Devices", null);
            }
        }

        public async Task<ActionResult> RemoveSection(int id, int deviceId, string sectionName)
        {
            try
            {
                var response = await _deviceService.DeleteSection(id);
                if (response.Status == ServerResponseStatus.SUCCESS)
                {
                    AddRedirectMessage(ServerResponseStatus.SUCCESS,
                        string.Format("Section '{0}' deleted!", sectionName));
                }
                else
                {
                    AddRedirectMessage(response.Status, response.Title, response.Message);
                }
                return RedirectToAction("Layouts", "Devices", new { id = deviceId });
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
                return RedirectToAction("Layouts", "Devices", new { id = deviceId });
            }
        }
    }
}