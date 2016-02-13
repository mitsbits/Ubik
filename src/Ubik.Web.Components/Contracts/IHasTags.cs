using System.Collections.Generic;

namespace Ubik.Web.Components.Contracts
{
    public interface IHasTags
    {
        IEnumerable<ITag> Tags { get; }
    }
}