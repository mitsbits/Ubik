using System.Collections.Generic;

namespace Ubik.Web.Components.Contracts
{
    public interface IDevice
    {
        string FriendlyName { get; }

        string Path { get; }

        ICollection<ISection> Sections { get; }

        DeviceRenderFlavor Flavor { get; }
    }
}