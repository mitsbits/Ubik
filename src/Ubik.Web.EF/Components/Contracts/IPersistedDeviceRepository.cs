using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;

namespace Ubik.Web.EF.Components.Contracts
{
   public interface IPersistedDeviceRepository : ICRUDRespoditory<PersistedDevice>, ISequenceRepository<PersistedDevice>
    {
    }
}
