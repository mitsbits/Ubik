using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Web.EF.Components;
using Ubik.Web.EF.Components.Contracts;

namespace Ubik.Web.Components.AntiCorruption.ViewModels.Devices
{
    public class SectionSaveModel
    {
        [Required]
        public int SectionId { get; set; }

        [Required]
        public int DeviceId { get; set; }

        [Required]
        public string FriendlyName { get; set; }

        [Required]
        public string Identifier { get; set; }

        [Required]
        public DeviceRenderFlavor ForFlavor { get; set; }
    }

    public class SectionViewModel : SectionSaveModel
    {
        public SectionViewModel()
        {
            ForFlavor = DeviceRenderFlavor.Empty;
            Slots = new HashSet<SlotViewModel>();
        }

        public ICollection<SlotViewModel> Slots { get; set; }
    }

    public class SectionViewModelBuilder : IViewModelBuilder<PersistedSection, SectionViewModel>
    {
        public SectionViewModel CreateFrom(PersistedSection entity)
        {
            return new SectionViewModel() { ForFlavor = entity.ForFlavor, FriendlyName = entity.FriendlyName, SectionId = entity.Id, DeviceId = entity.DeviceId, Identifier = entity.Identifier };
        }

        public void Rebuild(SectionViewModel model)
        {
            return;
        }
    }

    public class SectionViewModelCommand : IViewModelCommand<SectionSaveModel>
    {
        private readonly IPersistedSectionRepository _persistedSectionRepo;

        public SectionViewModelCommand(IPersistedSectionRepository persistedSectionRepo)
        {
            _persistedSectionRepo = persistedSectionRepo;
        }

        public async Task Execute(SectionSaveModel model)
        {
            PersistedSection data;
            if (model.SectionId != default(int))
            {
                data = await _persistedSectionRepo.GetAsync(x => x.Id == model.SectionId);
                data.ForFlavor = model.ForFlavor;
                data.FriendlyName = model.FriendlyName;
                data.Identifier = model.Identifier;
            }
            else
            {
                data = new PersistedSection() { FriendlyName = model.FriendlyName, DeviceId = model.DeviceId, ForFlavor = model.ForFlavor, Identifier = model.Identifier };
                data.Id = await _persistedSectionRepo.GetNext();
                await _persistedSectionRepo.CreateAsync(data);
                model.SectionId = data.Id;
            }
        }
    }
}