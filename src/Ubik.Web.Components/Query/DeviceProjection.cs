namespace Ubik.Web.Components.Query
{
    public class DeviceProjection<TKey>
    {
        public TKey Id { get; set; }

        public string FriendlyName { get; set; }

        public string Path { get; set; }

        public DeviceRenderFlavor Flavor { get; set; }

        public TKey SectionId { get; set; }

        public string SectionIdentifier { get; set; }

        public string SectionFriendlyName { get; set; }

        public DeviceRenderFlavor SectionForFlavor { get; set; }

        public bool SlotEnabled { get; set; }

        public int SlotOrdinal { get; set; }

        public int SlotFlavor { get; set; }

        public string ModuleType { get; set; }

        public string ModuleInfo { get; set; }
    }
}