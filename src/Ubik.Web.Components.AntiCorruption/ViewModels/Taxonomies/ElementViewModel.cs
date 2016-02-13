using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Infra.DataManagement;
using Ubik.Infra.Ext;
using Ubik.Web.EF.Components;
using Ubik.Web.EF.Components.Contracts;

namespace Ubik.Web.Components.AntiCorruption.ViewModels.Taxonomies
{
    public class ElementSaveModel
    {
        [Required]
        public int Id { get; set; } = default(int);

        [Required]
        public ComponentStateFlavor ComponentStateFlavor { get; set; } = ComponentStateFlavor.Empty;

        [Required]
        public int ParentId { get; set; } = default(int);

        [Required]
        public int Depth { get; set; } = default(int);

        [Required]
        public int DivisionId { get; set; } = default(int);

        [Required]
        public int TextualId { get; set; } = default(int);

        [Required]
        public virtual string Name { get; set; } = string.Empty;

        public virtual string Summary { get; set; } = string.Empty;
    }

    public class ElementViewModel : ElementSaveModel
    {
        public IReadOnlyDictionary<int, string> DivisionOptions { get; set; }
    }

    public class ElementViewModelBuilder : IViewModelBuilder<PersistedTaxonomyElement, ElementViewModel>
    {
        private readonly IPersistedTaxonomyDivisionRepository _divisionRepo;

        public ElementViewModelBuilder(IPersistedTaxonomyDivisionRepository divisionRepo)
        {
            _divisionRepo = divisionRepo;
        }

        public ElementViewModel CreateFrom(PersistedTaxonomyElement entity)
        {
            return new ElementViewModel()
            {
                ComponentStateFlavor = entity.ComponentStateFlavor,
                Depth = entity.Depth,
                DivisionId = entity.DivisionId,
                Id = entity.Id,
                Name = entity.Textual.Subject,
                Summary = (entity.Textual.Summary == null) ? string.Empty : entity.Textual.Summary.ToUTF8(),
                ParentId = entity.ParentId,
                TextualId = entity.Textual.Id
            };
        }

        public void Rebuild(ElementViewModel model)
        {
            var divisions = _divisionRepo.FindAsync(x => true,
                   new[]
                   {new OrderByInfo<PersistedTaxonomyDivision>() {Ascending = true, Property = m => m.Textual.Subject}}, 1,
                   100000, m => m.Textual).Result;
            model.DivisionOptions = divisions.ToDictionary(x => x.Id, x => x.Textual.Subject);
        }
    }

    public class ElementViewModelCommand : IViewModelCommand<ElementSaveModel>
    {
        private readonly IPersistedTaxonomyDivisionRepository _divisionRepo;
        private readonly IPersistedTextualRepository _textualRepo;
        private readonly IPersistedTaxonomyElementRepository _elementRepo;

        public ElementViewModelCommand(IPersistedTaxonomyDivisionRepository divisionRepo, IPersistedTextualRepository textualRepo, IPersistedTaxonomyElementRepository elementRepo)
        {
            _divisionRepo = divisionRepo;
            _textualRepo = textualRepo;
            _elementRepo = elementRepo;
        }

        public async Task Execute(ElementSaveModel model)
        {
            var isTransient = model.Id == default(int);
            if (isTransient)
            {
                var entity = new PersistedTaxonomyElement()
                {
                };
                await _elementRepo.CreateAsync(entity);
            }
            else
            {
                var entity = await _elementRepo.GetAsync(x => x.Id == model.Id, division => division.Textual);
                entity.Textual.Subject = model.Name;
                entity.Textual.Summary = model.Summary.ConvertUTF8ToBinary();
                await _elementRepo.UpdateAsync(entity);
            }
        }
    }
}