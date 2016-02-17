using System.Collections.Generic;
using Ubik.EF6.Contracts;
using Ubik.Web.Components;

namespace Ubik.Web.EF.Components
{
    public abstract class PersistedComponent : ISequenceBase
    {
        public virtual int Id { get; set; }

        public virtual ComponentStateFlavor ComponentStateFlavor { get; set; }
    }

    public class PersistedContent : PersistedComponent
    {
        public PersistedContent()
        {
            Tags = new HashSet<PersistedTag>();
        }

        public virtual int TextualId { get; set; }

        public virtual PersistedTextual Textual { get; set; }

        public virtual PersistedHtmlHead HtmlHead { get; set; }

        public virtual ICollection<PersistedTag> Tags { get; set; }
    }

    public class PersistedTag : PersistedComponent
    {
        public PersistedTag()
        {
            Contents = new HashSet<PersistedContent>();
        }

        public virtual string Value { get; set; }
        public virtual ICollection<PersistedContent> Contents { get; set; }
    }

    public class PersistedHtmlHead
    {
        public virtual int Id { get; set; }

        public virtual string MetasInfo { get; set; }

        public virtual string CanonicalURL { get; set; }

        public virtual string Slug { get; set; }
    }

    public class PersistedDevice : ISequenceBase
    {
        public PersistedDevice()
        {
            Sections = new HashSet<PersistedSection>();
            Flavor = DeviceRenderFlavor.Empty;
        }

        public int Id { get; set; }

        public string FriendlyName { get; set; }

        public string Path { get; set; }

        public DeviceRenderFlavor Flavor { get; set; }

        public ICollection<PersistedSection> Sections { get; set; }
    }

    public class PersistedSection : ISequenceBase
    {
        public PersistedSection()
        {
            Slots = new HashSet<PersistedSlot>();
        }

        public int Id { get; set; }

        public int DeviceId { get; set; }

        public string Identifier { get; set; }

        public string FriendlyName { get; set; }

        public DeviceRenderFlavor ForFlavor { get; set; }

        public ICollection<PersistedSlot> Slots { get; set; }
    }

    public class PersistedSlot
    {
        public int SectionId { get; set; }

        public bool Enabled { get; set; }

        public int Ordinal { get; set; }

        public string ModuleType { get; set; }

        public string ModuleInfo { get; set; }

        public virtual PersistedSection Section { get; set; }
    }
}