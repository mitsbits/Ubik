using System.Collections.Generic;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.BuildingBlocks.Contracts
{
    internal interface IDevicePageProvider
    {
        IDevice Current { get; }

        IEnumerable<ISection> ActiveSections { get; }
    }

    internal interface IContentPageProvider
    {
        IContent Current { get; }
    }
}