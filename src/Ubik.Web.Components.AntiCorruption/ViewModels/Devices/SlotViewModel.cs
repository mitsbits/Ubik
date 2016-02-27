using System.Collections.Generic;
using Ubik.Infra.Contracts;
using Ubik.Web.Components.Contracts;
using Ubik.Infra.Ext;
using Ubik.Web.BuildingBlocks.Contracts;

using Ubik.Web.Components.DTO;
using Ubik.Web.EF.Components;
using System.Linq;
using System;
using System.Threading.Tasks;
using Ubik.Web.Components.AntiCorruption.Contracts;
using Ubik.Web.EF.Components.Contracts;

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
                foreach (var t in descriptor.Default().Parameters)
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


    public class SlotViewModelCommand : IViewModelCommand<SlotSaveModel>
    {
        private readonly IResident _resident;

        private readonly IPersistedSectionRepository _sectionRepo;

        public SlotViewModelCommand(IResident resident,  IPersistedSectionRepository sectionRepo)
        {
            _resident = resident;

            _sectionRepo = sectionRepo;
        }
        public async Task Execute(SlotSaveModel model)
        {
            var module = Transform(model);
            var section = await _sectionRepo.GetAsync(x => x.Id == model.SectionId, x => x.Slots);
            var hit = section.Slots.FirstOrDefault(x => x.Ordinal == model.Ordinal);
            if (hit != null)
            {
                hit.Enabled = model.Enabled;
                hit.ModuleType = model.ModuleType;
                hit.ModuleInfo = module.GetXmlString();
            }
            else
            {
                hit = new PersistedSlot()
                {
                    Enabled = model.Enabled,
                    ModuleType = model.ModuleType,
                    Ordinal = model.Ordinal,
                    SectionId = model.SectionId,
                    ModuleInfo = module.GetXmlString()
                };
                await _sectionRepo.AddSlot(hit);
            }
        }


        private Task<BasePartialModule> Transform(SlotSaveModel config)
        {
            var descriptor = _resident.Modules.Installed.Single(x => x.Default().GetType().FullName == config.FullName);
            var module = descriptor.Default();
            module.Parameters.Clear();
            foreach (var p in config.Parameters)
            {
                module.Parameters.Add(p);
            }
            return Task.FromResult(module);
        }
    }
}