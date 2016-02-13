using Ubik.Infra.Contracts;
using Ubik.Web.Components.Domain;
using Ubik.Web.Components.Query;

namespace Ubik.Web.Components.Contracts
{
    public interface IDeviceRepository<TKey>
    {
        Device<TKey> Get(int id);
    }

    public interface IDeviceQueryRepository<TKey> : IQueryRepository<Device<TKey>, DeviceProjection<TKey>>
    {
    }
}