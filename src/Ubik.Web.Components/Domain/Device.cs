using System.Collections.Generic;
using System.Linq;
using Ubik.Web.Components.Contracts;
using Ubik.Web.Components.Exc;

namespace Ubik.Web.Components.Domain
{
    public class Device<TKey> : EntityBase<TKey>, IDevice
    {
        internal Device()
        {
            Sections = new HashSet<Section<TKey>>();
        }

        public Device(TKey id, string friendlyName, string path)
            : this()
        {
            Id = id;
            FriendlyName = friendlyName;
            Path = path;
        }

        public string FriendlyName { get; private set; }

        public string Path { get; private set; }

        ICollection<ISection> IDevice.Sections { get { return Sections.Cast<ISection>().ToList(); } }
        public ICollection<Section<TKey>> Sections { get; private set; }
        private DeviceRenderFlavor _flavor = DeviceRenderFlavor.Empty;

        public DeviceRenderFlavor Flavor
        {
            get { return _flavor; }
        }

        public void RemoveSection(TKey id)
        {
            if (Sections.All(s => !s.Id.Equals(id)))
                throw new InvariantException(
                    string.Format("Device:{0} does not hold a section with id {1}. Deletion fails.",
                        Id, id));
            var section = Sections.FirstOrDefault(s => !s.Id.Equals(id));

            Sections.Remove(section);
        }

        public void AddSection(Section<TKey> section)
        {
            if (Sections.Any(s => !s.Id.Equals(section.Id)))
                throw new InvariantException(
                string.Format("Device:{0} section with id {1} exists. Add fails.",
               Id, section.Id));
            Sections.Add(section);
        }

        public void DefineSectionSlot(int sectionId, int index, bool enabled, BasePartialModule module)
        {
            var existingSection = Sections.Single(s => s.Id.Equals(sectionId)) as ISection;
            if (existingSection == null) return;
            var sectionIdentifier =
                existingSection.Identifier;
            if (Sections.All(s => !s.Id.Equals(sectionId)))
                throw new InvariantException(
string.Format("Device:{0} section with id {1} exists. Slot definition fails.",
Id, sectionId));
            var section = Sections.Single(s => s.Id.Equals(sectionId));
            if (section != null) section.DefineSlot(new SectionSlotInfo(sectionIdentifier, enabled, index), module);
        }

        public void SetFlavor(DeviceRenderFlavor flavor)
        {
            if (Flavor != flavor)
                _flavor = flavor;
        }

        public void SetPath(string path)
        {
            Path = path;
        }

        public void SetFriendlyName(string friendlyName)
        {
            FriendlyName = friendlyName;
        }

        public void SetSectionFlavor(int sectionId, DeviceRenderFlavor flavor)
        {
            if (Sections.Any(s => s.Id.Equals(sectionId)))
                throw new InvariantException(
                    string.Format("Device:{0} section with id {1} exists. Set section flavor fails.",
                    Id, sectionId));
            var section = Sections.Single(s => s.Id.Equals(sectionId));
            if (section != null && section.ForFlavor == flavor)
                throw new InvariantException(
                    string.Format("Device:{0} flavor already has value of {1} for section with id {2}. Set flavor fails.",
                    Id, flavor, sectionId));
            if (section != null) section.SetForFlavor(flavor);
        }
    }
}