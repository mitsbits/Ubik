using System.Collections.Generic;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Web.Components.AntiCorruption.ViewModels.Taxonomies;

namespace Ubik.Web.Components.AntiCorruption.Contracts
{
    public interface ITaxonomiesViewModelService
    {
        Task<DivisionViewModel> DivisionModel(int id);

        Task<IPagedResult<DivisionViewModel>> DivisionModels(int pageNumber, int pageSize);

        Task Execute(DivisionSaveModel model);

        Task<ElementViewModel> ElementModel(int id);

        Task<IPagedResult<ElementViewModel>> ElementModels(int pageNumber, int pageSize);

        Task<IEnumerable<ElementViewModel>> ElementModelsFragment(int parentId);

        Task<IEnumerable<ElementViewModel>> ElementModelsDivisionFragment(int divisionId);

        Task<ElementHierarchy> HierarchicalElementModelsFragment(int parentId);

        Task<ElementHierarchy> HierarchicalElementModelsDivisionFragment(int divisionId);

        Task Execute(ElementSaveModel model);
    }
}