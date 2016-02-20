using System.Collections.Generic;
using Ubik.Infra.Contracts;
using Ubik.Infra.Ext;
using Ubik.Web.BuildingBlocks.Contracts;
using Ubik.Web.Components.Contracts;
using Ubik.Web.Components.DTO;
using Ubik.Web.EF.Components;
using System.Linq;

namespace Ubik.Web.Components.AntiCorruption.ViewModels.Devices
{
    public class SlotSaveModel
    {
        private readonly ICollection<Tiding> _parameters;
        public SlotSaveModel()
        {
            _parameters = new HashSet<Tiding>();
        }

        public int SectionId { get; set; }

        public bool Enabled { get; set; }

        public int Ordinal { get; set; }

        public string ModuleType { get; set; }

        public string FriendlyName { get; set; }
        public string Summary { get; set; }

        public string ModuleGroup { get; set; }

        public string FullName { get; set; }

        public ICollection<Tiding> Parameters { get { return _parameters; } }

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

            model.AvailableModules = _resident.Modules.Installed;


            if (!string.IsNullOrWhiteSpace(entity.ModuleInfo))
            {
               var module = entity.ModuleInfo.XmlDeserializeFromString<BasePartialModule>();
                var descriptor = model.AvailableModules.Single(x => x.GetType().FullName == module.GetType().FullName);
                model.ModuleGroup = descriptor.ModuleGroup;
                model.Parameters.Clear();
                foreach(var t in descriptor.Default().Parameters)
                {
                    model.Parameters.Add(t);
                }
                model.Summary = descriptor.Summary;
                model.FriendlyName = descriptor.FriendlyName;

            }
        
            return model;
        }

        public void Rebuild(SlotViewModel model)
        {
            return;
        }
    }
}