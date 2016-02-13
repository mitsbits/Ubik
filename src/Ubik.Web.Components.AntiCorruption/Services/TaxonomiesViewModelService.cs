using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Infra.DataManagement;
using Ubik.Web.Components.AntiCorruption.Contracts;
using Ubik.Web.Components.AntiCorruption.ViewModels.Taxonomies;
using Ubik.Web.EF.Components;
using Ubik.Web.EF.Components.Contracts;

namespace Ubik.Web.Components.AntiCorruption.Services
{
    public class TaxonomiesViewModelService : ITaxonomiesViewModelService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IPersistedTaxonomyDivisionRepository _divisionRepo;
        private readonly IPersistedTaxonomyElementRepository _elementRepo;

        private readonly IViewModelBuilder<PersistedTaxonomyDivision, DivisionViewModel> _divisionBuilder;
        private readonly IViewModelBuilder<PersistedTaxonomyElement, ElementViewModel> _elementBuilder;

        private readonly IViewModelCommand<DivisionSaveModel> _divisionCommand;
        private readonly IViewModelCommand<ElementSaveModel> _elementCommand;

        public TaxonomiesViewModelService(IDbContextScopeFactory dbContextScopeFactory,
            IPersistedTaxonomyDivisionRepository divisionRepo, IViewModelCommand<DivisionSaveModel> divisionCommand, IViewModelCommand<ElementSaveModel> elementCommand, IPersistedTaxonomyElementRepository elementRepo)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _divisionRepo = divisionRepo;
            _divisionCommand = divisionCommand;
            _elementCommand = elementCommand;
            _elementRepo = elementRepo;

            _divisionBuilder = new DivisionViewModelBuilder();
            _elementBuilder = new ElementViewModelBuilder(_divisionRepo);
        }

        #region Divisions

        public async Task<DivisionViewModel> DivisionModel(int id)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                DivisionViewModel model;
                if (id == default(int))
                {
                    model =
                        _divisionBuilder.CreateFrom(new PersistedTaxonomyDivision() { Textual = new PersistedTextual() });
                    _divisionBuilder.Rebuild(model);
                    return await Task.FromResult(model);
                }

                var entity = await _divisionRepo.GetAsync(x => x.Id == id, division => division.Textual);
                if (entity == null) throw new Exception(string.Format("no taxonomy division with id:{0}", id));
                model = _divisionBuilder.CreateFrom(entity);
                _divisionBuilder.Rebuild(model);
                return model;
            }
        }

        public async Task<IPagedResult<DivisionViewModel>> DivisionModels(int pageNumber, int pageSize)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var source = await _divisionRepo.FindAsync(x => true,
                    new[]
                    {
                        new OrderByInfo<PersistedTaxonomyDivision>()
                        {
                            Ascending = true,
                            Property = m => m.Textual.Subject
                        },
                    },
                    pageNumber, pageSize, m => m.Textual);

                var bucket = new List<DivisionViewModel>();
                foreach (var persistedTaxonomyDivision in source.Data)
                {
                    var model = _divisionBuilder.CreateFrom(persistedTaxonomyDivision);
                    _divisionBuilder.Rebuild(model);
                    bucket.Add(model);
                }
                return new PagedResult<DivisionViewModel>(bucket, source.PageNumber, source.PageSize,
                    source.TotalRecords);
            }
        }

        public async Task Execute(DivisionSaveModel model)
        {
            using (var db = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted))
            {
                await _divisionCommand.Execute(model);
                await db.SaveChangesAsync();
            }
        }

        public async Task<ElementViewModel> ElementModel(int id)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                ElementViewModel model;
                if (id == default(int))
                {
                    model =
                        _elementBuilder.CreateFrom(new PersistedTaxonomyElement()
                        {
                            ComponentStateFlavor = ComponentStateFlavor.Empty,
                            Depth = default(int),
                            DivisionId = default(int),
                            Id = default(int),
                            ParentId = default(int),
                            TextualId = default(int),
                            Division = new PersistedTaxonomyDivision()
                            {
                                Id = default(int),
                                TextualId = default(int),
                                Textual = new PersistedTextual()
                                {
                                    Id = default(int),
                                    Subject = string.Empty,
                                    Summary = new byte[] { }
                                }
                            },
                            Textual = new PersistedTextual()
                            {
                                Id = default(int),
                                Subject = string.Empty,
                                Summary = new byte[] { }
                            }
                        });

                    _elementBuilder.Rebuild(model);
                    return await Task.FromResult(model);
                }

                var entity = await _elementRepo.GetAsync(x => x.Id == id,
                    element => element.Textual,
                    element => element.Division,
                    element => element.Division.Textual);
                if (entity == null) throw new Exception(string.Format("no taxonomy division with id:{0}", id));
                model = _elementBuilder.CreateFrom(entity);
                _elementBuilder.Rebuild(model);
                return model;
            }
        }

        public async Task<IPagedResult<ElementViewModel>> ElementModels(int pageNumber, int pageSize)
        {
            var entites = await _elementRepo.FindAsync(x => true,
                new[]
                {
                    new OrderByInfo<PersistedTaxonomyElement>()
                    {
                        Ascending = true,
                        Property = element => element.Division.Textual.Subject
                    },
                    new OrderByInfo<PersistedTaxonomyElement>()
                    {
                        Ascending = true, Property = element => element.Depth
                    },
                    new OrderByInfo<PersistedTaxonomyElement>()
                    {
                        Ascending = true,
                        Property = element => element.Textual.Subject
                    }
                }, pageNumber, pageSize,
                element => element.Textual,
                element => element.Division,
                element => element.Division.Textual);

            return new PagedResult<ElementViewModel>(entites.Data.Select(x => _elementBuilder.CreateFrom(x)),
                entites.PageNumber, entites.PageSize, entites.TotalRecords);
        }

        public async Task<IEnumerable<ElementViewModel>> ElementModelsFragment(int parentId)
        {
            var entities = await _elementRepo.ElementsFragment(parentId);
            entities = SortItemsBasedOnHierarchy<PersistedTaxonomyElement>(entities);
            return entities.Select(x => _elementBuilder.CreateFrom(x));
        }

        public async Task<IEnumerable<ElementViewModel>> ElementModelsDivisionFragment(int divisionId)
        {
            var entities = await _elementRepo.ElementsForDivisionFragment(divisionId);
            entities = SortItemsBasedOnHierarchy<PersistedTaxonomyElement>(entities);
            return entities.Select(x => _elementBuilder.CreateFrom(x));
        }

        //TODO: move this to an extension
        private IEnumerable<TEntity> SortItemsBasedOnHierarchy<TEntity>(IEnumerable<IHasParent<int>> collection) where TEntity : IHasParent<int>
        {
            var result = new List<TEntity>();
            var minDepth = collection.Min(x => x.Depth);

            foreach (var hasParent in collection.Where(x => x.Depth == minDepth).OrderBy(x => (typeof(TEntity).GetInterface(typeof(IWeighted).Name) != null) ? ((IWeighted)x).Weight : x.Id))
            {
                Traverse<TEntity>(ref result, hasParent, collection);
            }
            return result;
        }

        private static void Traverse<TEntity>(ref List<TEntity> result, IHasParent<int> hasParent, IEnumerable<IHasParent<int>> collection) where TEntity : IHasParent<int>
        {
            result.Add((TEntity)hasParent);
            foreach (var child in collection.Where(x => x.ParentId == hasParent.ParentId).OrderBy(x => (typeof(TEntity).GetInterface(typeof(IWeighted).Name) != null) ? ((IWeighted)x).Weight : x.Id))
            {
                Traverse(ref result, child, collection);
            }
        }

        public async Task<ElementHierarchy> HierarchicalElementModelsFragment(int parentId)
        {
            var entities = await _elementRepo.ElementsFragment(parentId);
            var hierarchy = new ElementHierarchy(entities);
            return hierarchy;
        }

        public async Task<ElementHierarchy> HierarchicalElementModelsDivisionFragment(int divisionId)
        {
            var entities = await _elementRepo.ElementsFragment(divisionId);
            var hierarchy = new ElementHierarchy(entities);
            return hierarchy;
        }

        public async Task Execute(ElementSaveModel model)
        {
            using (var db = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted))
            {
                await _elementCommand.Execute(model);
                await _divisionRepo.UpdateHierarchy(model.DivisionId);
                await db.SaveChangesAsync();
            }
        }

        #endregion Divisions
    }
}