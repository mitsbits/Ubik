using System.Threading.Tasks;
using Ubik.Infra.Contracts;

namespace Ubik.Web.EF.Components.Contracts
{
    public interface IPersistedTaxonomyDivisionRepository : ICRUDRespoditory<PersistedTaxonomyDivision>
    {
        Task UpdateHierarchy(int id);
    }
}