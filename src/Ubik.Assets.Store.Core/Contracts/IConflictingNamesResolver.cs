using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Assets.Store.Core.Contracts
{
    public interface IConflictingNamesResolver
    {
        Task<string> Resolve(string filename);
    }


}
