using System.Collections.Generic;
using System.Threading.Tasks;
using Ubik.Web.Components.AntiCorruption.ViewModels.Devices;

namespace Ubik.Web.Components.AntiCorruption.Contracts
{
    public interface IDeviceAdministrationViewModelService
    {
        Task<DeviceViewModel> DeviceModel(int id);

        Task<IEnumerable<DeviceViewModel>> DeviceModels();

        Task Execute(DeviceSaveModel model);

        Task Execute(SectionSaveModel model);

        Task<BasePartialModule> Transform(SlotViewModel config);
    }
}