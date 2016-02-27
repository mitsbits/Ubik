using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Web.Components.Domain;

namespace Ubik.Web.EF.Components.Contracts
{
   public interface IPersistedDeviceRepository : ICRUDRespoditory<PersistedDevice>, ISequenceRepository<PersistedDevice>
    {
    }

    public interface IPersistedSectionRepository : ICRUDRespoditory<PersistedSection>, ISequenceRepository<PersistedSection>
    {
        Task AddSlot(PersistedSlot slot);
    }
}
