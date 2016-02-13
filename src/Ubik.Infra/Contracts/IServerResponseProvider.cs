using System.Collections.Generic;

namespace Ubik.Infra.Contracts
{
    public interface IServerResponseProvider
    {
        ICollection<IServerResponse> Messages { get; }
    }
}