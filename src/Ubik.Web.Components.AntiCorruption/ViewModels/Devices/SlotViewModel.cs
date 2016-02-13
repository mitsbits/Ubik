using System.Collections.Generic;
using Ubik.Infra.Contracts;
using Ubik.Infra.Ext;
using Ubik.Web.BuildingBlocks.Contracts;
using Ubik.Web.Components.Contracts;
using Ubik.Web.EF.Components;

namespace Ubik.Web.Components.AntiCorruption.ViewModels.Devices
{
    public class SlotSaveModel
    {
        public int SectionId { get; set; }

        public bool Enabled { get; set; }

        public int Ordinal { get; set; }

        public string ModuleType { get; set; }

        public BasePartialModule Module { get; set; }
    }

    public class SlotViewModel : SlotSaveModel
    {
        public string SectionIdentifier { get; set; }
        public IEnumerable<IModuleDescriptor> AvailableModules { get; set; }
    }

    public class SlotViewModelBuilder : IViewModelBuilder<PersistedSlot, SlotViewModel>
    {
        private readonly IResident _resident;

        public SlotViewModelBuilder(IResident resident)
        {
            _resident = resident;
        }

        public SlotViewModel CreateFrom(PersistedSlot entity)
        {
            var model = new SlotViewModel()
            {
                SectionIdentifier = entity.Section.Identifier,
                Enabled = entity.Enabled,
                Ordinal = entity.Ordinal,
                ModuleType = entity.ModuleType
            };
            if (!string.IsNullOrWhiteSpace(entity.ModuleInfo))
            {
                model.Module = entity.ModuleInfo.XmlDeserializeFromString<BasePartialModule>();
            }
            model.AvailableModules = _resident.Modules.Installed;
            return model;
        }

        public void Rebuild(SlotViewModel model)
        {
            return;
        }
    }
}