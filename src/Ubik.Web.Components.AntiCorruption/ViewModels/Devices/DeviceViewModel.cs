using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Web.BuildingBlocks.Contracts;
using Ubik.Web.Components.Contracts;
using Ubik.Web.EF.Components;

namespace Ubik.Web.Components.AntiCorruption.ViewModels.Devices
{
    public class DeviceSaveModel
    {
        public DeviceSaveModel()
        {
            Sections = new HashSet<SectionViewModel>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public string FriendlyName { get; set; }

        [Required]
        public string Path { get; set; }

        public DeviceRenderFlavor Flavor { get; set; }

        public ICollection<SectionViewModel> Sections { get; set; }
    }

    public class DeviceViewModel : DeviceSaveModel
    {
        public DeviceViewModel()
            : base()
        {
        }

        public IEnumerable<IModuleDescriptor> AvailableModules { get; set; }
    }

    public class DeviceViewModelBuilder : IViewModelBuilder<PersistedDevice, DeviceViewModel>
    {
        private readonly IResident _resident;

        public DeviceViewModelBuilder(IResident resident)
        {
            _resident = resident;
        }

        public DeviceViewModel CreateFrom(PersistedDevice entity)
        {
            var model = new DeviceViewModel
            {
                Flavor = entity.Flavor,
                FriendlyName = entity.FriendlyName,
                Id = entity.Id,
                Path = entity.Path,
                Sections = entity.Sections.Select(s => new SectionViewModel()
                {
                    ForFlavor = s.ForFlavor,
                    FriendlyName = s.FriendlyName,
                    SectionId = s.Id,
                    DeviceId = entity.Id,
                    Identifier = s.Identifier
                }).ToList(),
            };
            model.AvailableModules = _resident.Modules.Installed;
            return model;
        }

        public void Rebuild(DeviceViewModel model)
        {
            return;
        }
    }

    public class DeviceViewModelCommand : IViewModelCommand<DeviceSaveModel>
    {
        private readonly ICRUDRespoditory<PersistedDevice> _persistedDeviceRepo;

        public DeviceViewModelCommand(ICRUDRespoditory<PersistedDevice> persistedDeviceRepo)
        {
            _persistedDeviceRepo = persistedDeviceRepo;
        }

        public async Task Execute(DeviceSaveModel model)
        {
            PersistedDevice data;
            if (model.Id != default(int))
            {
                data = await _persistedDeviceRepo.GetAsync(x => x.Id == model.Id);
                data.Flavor = model.Flavor;
                data.FriendlyName = model.FriendlyName;
                data.Path = model.Path;
                await _persistedDeviceRepo.UpdateAsync(data);
            }
            else
            {
                data = new PersistedDevice() { FriendlyName = model.FriendlyName, Flavor = model.Flavor, Path = model.Path };
                await _persistedDeviceRepo.CreateAsync(data);
            }
        }
    }
}