using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Infra.DataManagement;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.Components.Contracts
{
    public interface IDeviceAdministrationService<TKey>
    {
        Task<PagedResult<Device<TKey>>> All(int pageNumber, int pageSize);

        Task<Device<TKey>> Get(TKey id);

        Task<IServerResponse> DeleteSection(int id);
    }
}