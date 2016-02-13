namespace Ubik.Web.Components.Contracts
{
    public interface ISectionSlotInfo
    {
        string SectionIdentifier { get; }
        bool Enabled { get; }
        int Ordinal { get; }
    }
}