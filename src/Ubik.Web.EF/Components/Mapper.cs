using System.Collections.Generic;
using System.Linq;
using Ubik.Infra.Ext;
using Ubik.Web.Components;
using Ubik.Web.Components.Domain;
using Ubik.Web.Components.Query;

namespace Ubik.Web.EF.Components
{
    public static class Mapper
    {
        public static Device<int> MapToDomain(PersistedDevice source)
        {
            var result = new Device<int>(source.Id, source.FriendlyName, source.Path);
            result.SetFlavor(source.Flavor);
            foreach (var persistedSection in source.Sections)
            {
                result.AddSection(MapToDomain(persistedSection));
            }
            return result;
        }

        public static Section<int> MapToDomain(PersistedSection source)
        {
            var result = new Section<int>(source.Id, source.Identifier, source.FriendlyName, (DeviceRenderFlavor)source.ForFlavor);

            foreach (var persistedSlot in source.Slots)
            {
                var module = persistedSlot.ModuleInfo.XmlDeserializeFromString<BasePartialModule>();
                result.DefineSlot(new SectionSlotInfo(source.Identifier, persistedSlot.Enabled, persistedSlot.Ordinal),
                    module);
            }
            return result;
        }

        public static Content<int> MapToDomain(PersistedContent source)
        {
            var result = new Content<int>(source.Id, MapToDomain(source.Textual), source.HtmlHead.CanonicalURL);
            result.SetState((ComponentStateFlavor)source.ComponentStateFlavor);
            var metas = source.HtmlHead.MetasInfo.XmlDeserializeFromString<ICollection<Meta>>();
            if (metas != null && metas.Any())
            {
                foreach (var meta in metas)
                {
                    result.HtmlHead.Metas.Add(meta);
                }
            }
            return result;
        }

        public static Textual MapToDomain(PersistedTextual source)
        {
            var result = new Textual(source.Subject, source.Summary.ToUTF8(), source.Body.ToUTF8());
            return result;
        }

        public static IEnumerable<Device<TKey>> Map<TKey>(IEnumerable<DeviceProjection<TKey>> source)
        {
            var groups = source.GroupBy(x => x.Id);
            foreach (var @group in groups)
            {
                var head = @group.FirstOrDefault();
                if (head == null) continue;
                var device = new Device<TKey>(head.Id, head.FriendlyName, head.Path);
                device.SetFlavor(head.Flavor);
                foreach (var sectionGroup in @group.GroupBy(x => x.SectionId))
                {
                    var sectionHead = sectionGroup.FirstOrDefault();
                    if (sectionHead == null) continue;
                    var section = new Section<TKey>(sectionHead.SectionId, sectionHead.SectionIdentifier,
                        sectionHead.SectionFriendlyName, sectionHead.SectionForFlavor);
                    foreach (var slot in sectionGroup.OrderBy(x => x.SlotOrdinal))
                    {
                        section.Slots.Add(new Slot(
                            new SectionSlotInfo(slot.SectionIdentifier, slot.SlotEnabled, slot.SlotOrdinal),
                            slot.ModuleInfo.XmlDeserializeFromString<BasePartialModule>()));
                    }
                    device.AddSection(section);
                }
                yield return device;
            }
        }
    }
}