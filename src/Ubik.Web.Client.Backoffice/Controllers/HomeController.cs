﻿using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;
using Ubik.Domain.Core;
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
    }
}