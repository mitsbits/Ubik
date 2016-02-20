using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Web.BuildingBlocks.Contracts;
using Ubik.Web.Components;

namespace Ubik.Web.Client.Backoffice.Controllers.Api
{
    public class DeviceOperationsController : BackofficeOperationsController
    {
        private readonly IResident _resident;

        public DeviceOperationsController(IResident resident) { _resident = resident; }

        [Route("api/backoffice/devices/mod-config/{id?}")]
        [HttpGet]
        public Task<BasePartialModule> GetModuleConfig(string id)
        {
            var hit = _resident.Modules.Installed.FirstOrDefault(x => x.Default().GetType().FullName == id);

            return Task.FromResult(hit.Default());
        }
    }
}
