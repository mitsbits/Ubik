using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;
using Ubik.Assets.Store.Core.Contracts;
using Ubik.Domain.Core;
using Ubik.Infra.Ext;
using Ubik.Web.Basis.Contracts;

namespace Ubik.Web.Client.Backoffice.Controllers
{
    public class HomeController : BackofficeController
    {
        public async Task<ActionResult> Index()
        {
            SetContentPage(new BackofficeContent() { Title = "Hey from back office" });
            await _eventDispatcher.Publish(new ContentSetEvent() { Title = "Hey from back office" });
            return View();
        }

        public HomeController(IErrorLogManager errorLogManager, IEventBus eventDispatcher) : base(errorLogManager, eventDispatcher)
        {
        }


        [HttpPost]
        public async Task<IActionResult> Upload([FromServices]IAssetService<int> store, IFormFile file)
        {
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var stream = file.OpenReadStream();
            var data = stream.ReadToEnd();
            var result = await store.AddNewVersion(31, data, fileName);

            return RedirectToAction("index");
        }
    }
}