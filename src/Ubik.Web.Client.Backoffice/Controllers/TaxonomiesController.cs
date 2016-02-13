using Microsoft.AspNet.Mvc;
using System;
using System.Threading.Tasks;
using Ubik.Domain.Core;
using Ubik.Infra;
using Ubik.Web.Basis.Contracts;
using Ubik.Web.Components.AntiCorruption.Contracts;
using Ubik.Web.Components.AntiCorruption.ViewModels.Taxonomies;

namespace Ubik.Web.Client.Backoffice.Controllers
{
    public class TaxonomiesController : BackofficeController
    {
        private readonly ITaxonomiesViewModelService _viewModelService;

        public TaxonomiesController(IErrorLogManager errorLogManager, IEventBus eventDispatcher, ITaxonomiesViewModelService viewModelService) : base(errorLogManager, eventDispatcher)
        {
            _viewModelService = viewModelService;
        }

        public async Task<ActionResult> Divisions(int? id)
        {
            try
            {
                return !id.HasValue
                 ? View(await _viewModelService.DivisionModels(Pager.Current, Pager.RowCount))
                 : View(await _viewModelService.DivisionModel(id.Value));
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateDivision(DivisionViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    AddRedirectMessage(ModelState);
                }
                await _viewModelService.Execute(model);
                AddRedirectMessage(ServerResponseStatus.SUCCESS, string.Format("Division '{0}' saved!", model.Name));
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
            }
            return RedirectToAction("divisions", "taxonomies", null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateElement(ElementViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    AddRedirectMessage(ModelState);
                }
                await _viewModelService.Execute(model);
                AddRedirectMessage(ServerResponseStatus.SUCCESS, string.Format("Element '{0}' saved!", model.Name));
                return RedirectToAction("elements", "taxonomies", new { id = model.Id, divisionId = model.DivisionId });
            }
            catch (Exception ex)
            {
                AddRedirectMessage(ex);
            }
            return RedirectToAction("divisions", "taxonomies", null);
        }

        [HttpGet]
        public async Task<ActionResult> AddElementToDivision(int divisionId)
        {
            var model = await _viewModelService.ElementModel(0);
            model.DivisionId = divisionId;
            return PartialView("Partials/ElementDetails", model);
        }
    }
}