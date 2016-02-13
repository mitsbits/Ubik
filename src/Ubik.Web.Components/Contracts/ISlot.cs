namespace Ubik.Web.Components.Contracts
{
    public interface ISlot
    {
        ISectionSlotInfo SectionSlotInfo { get; }

        BasePartialModule Module { get; }
    }
}