using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ubik.Assets.Store.Core.Contracts;
using Ubik.Domain.Core;
using Ubik.Infra.Ext;
using Ubik.Postal;
using Ubik.Postal.Contracts;
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
            var result = await store.Create(fileName, Assets.Store.Core.AssetState.Active, data, fileName);

            return RedirectToAction("index");
        }


        public async Task<IActionResult> SendMail([FromServices]IEmailService service)
        {
            try
            {
                var path = @"K:\Users\mitsbits\Desktop\k.pdf";
                var attachment = new FileAttachmentInfo(path, @"application\pdf", "test.pdf");
                await service.SendSingleMail(new MailAddress("mitsbits@gmail.com", "mr mitsbits"), new MailAddress("d_bitsanis@hotmail.com"), "test", @"hey <b>there!</b>", new List<IAttachmentInfo>(new[] { attachment }));

            }
            catch (Exception ex)
            {
                throw;
            }

            return RedirectToAction("index");


        }
    }
}