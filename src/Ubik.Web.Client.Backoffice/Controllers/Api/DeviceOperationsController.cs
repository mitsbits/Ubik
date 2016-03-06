using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Web.BuildingBlocks.Contracts;
using Ubik.Web.Components;
using Ubik.Web.Components.Contracts;
using Ubik.Web.Components.DTO;

namespace Ubik.Web.Client.Backoffice.Controllers.Api
{
    public class DeviceOperationsController : BackofficeOperationsController
    {
        private readonly IResident _resident;

        public DeviceOperationsController(IResident resident) { _resident = resident; }

        [Route("api/backoffice/devices/mod-config/{id?}")]
        [HttpGet]
        public Task<ModuleConfig> GetModuleConfig(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return Task.FromResult(ModuleConfig.Empty());
            var hit = _resident.Modules.Installed.FirstOrDefault(x => x.GetType().FullName == id);
            if (hit == null)
            {
                hit = _resident.Modules.Installed.FirstOrDefault(x => x.Default().GetType().FullName == id);
            }
            return Task.FromResult(new ModuleConfig(hit, hit.Default()));
        }

        public sealed class ModuleConfig
        {
            private ModuleConfig() { }
            public ModuleConfig(IModuleDescriptor descriptor, BasePartialModule instance)
            {
                FriendlyName = descriptor.FriendlyName;
                Summary = descriptor.Summary;
                ModuleType = descriptor.ModuleType.Flavor;
                ModuleGroup = descriptor.ModuleGroup;
                Parameters = instance.Parameters;

            }

            public static ModuleConfig Empty()
            {
                return new ModuleConfig() { FriendlyName = string.Empty, ModuleGroup = string.Empty, ModuleType = string.Empty, Summary = string.Empty, Parameters = new Tidings() };
            }
            public string FriendlyName { get; set; }
            public string Summary { get; set; }

            public string ModuleType { get; set; }

            public string ModuleGroup { get; set; }

            public Tidings Parameters { get; set; }
        }
    }
}
