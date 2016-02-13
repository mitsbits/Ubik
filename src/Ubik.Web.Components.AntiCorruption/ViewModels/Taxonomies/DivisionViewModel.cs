using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Infra.Ext;
using Ubik.Web.EF.Components;
using Ubik.Web.EF.Components.Contracts;

namespace Ubik.Web.Components.AntiCorruption.ViewModels.Taxonomies
{
    public class DivisionSaveModel
    {
        [Required]
        public int Id { get; set; }

        public int TextualId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Summary { get; set; }
    }

    public class DivisionViewModel : DivisionSaveModel
    {
    }

    public class DivisionViewModelBuilder : IViewModelBuilder<PersistedTaxonomyDivision, DivisionViewModel>
    {
        public DivisionViewModel CreateFrom(PersistedTaxonomyDivision entity)
        {
            return new DivisionViewModel()
            {
                Id = entity.Id,
                Name = entity.Textual.Subject,
                Summary = (entity.Textual.Summary == null) ? string.Empty : entity.Textual.Summary.ToUTF8()
            };
        }

        public void Rebuild(DivisionViewModel model)
        {
            return;
        }
    }

    public class DivisionViewModelCommand : IViewModelCommand<DivisionSaveModel>
    {
        private readonly IPersistedTaxonomyDivisionRepository _repo;
        private readonly IPersistedTextualRepository _textualRepo;

        public DivisionViewModelCommand(IPersistedTaxonomyDivisionRepository repo, IPersistedTextualRepository textualRepo)
        {
            _repo = repo;
            _textualRepo = textualRepo;
        }

        public async Task Execute(DivisionSaveModel model)
        {
            var isTransient = model.Id == default(int);
            if (isTransient)
            {
                var entity = new PersistedTaxonomyDivision()
                {
                    Textual = new PersistedTextual()
                    {
                        Subject = model.Name,
                        Summary = (string.IsNullOrWhiteSpace(model.Summary)) ? new byte[] { } : model.Summary.ConvertUTF8ToBinary()
                    }
                };
                await _repo.CreateAsync(entity);
            }
            else
            {
                var entity = await _repo.GetAsync(x => x.Id == model.Id, division => division.Textual);
                entity.Textual.Subject = model.Name;
                entity.Textual.Summary = model.Summary.ConvertUTF8ToBinary();
                await _repo.UpdateAsync(entity);
            }
        }
    }
}