using System.Collections.Generic;

namespace Ubik.Web.Components.Contracts
{
    public interface ISection
    {
        string Identifier { get; }

        string FriendlyName { get; }

        ICollection<ISlot> Slots { get; }

        DeviceRenderFlavor ForFlavor { get; }
    }
}