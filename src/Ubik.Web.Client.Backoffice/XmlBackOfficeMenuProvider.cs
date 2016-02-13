using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Ubik.Infra.Contracts;
using Ubik.Web.Basis.Navigation;
using Ubik.Web.Client.Backoffice.Contracts;

namespace Ubik.Web.Client.Backoffice
{
    public class XmlBackOfficeMenuProvider : IBackOfficeMenuProvider, IRequiresInitialization
    {
        private readonly XDocument _document;
        private readonly ICollection<NavigationElementDto> _data;
        private BackofficeNavigationElements _menu;

        internal XmlBackOfficeMenuProvider(string content)
        {
            _document = XDocument.Load(new StringReader(content));
            _data = new HashSet<NavigationElementDto>();
            Initialize();
        }

        public ICollection<NavigationElementDto> Raw { get { return _data; } }

        public BackofficeNavigationElements Menu
        {
            get
            {
                if (_menu == null)
                {
                    var roots =
                        _data.Where(x => x.ParentId == 0)
                            .OrderBy(x => x.Weight)
                            .Select(x => new BackofficeNavigationElement(_data, x.Id))
                            .ToList();
                    _menu = new BackofficeNavigationElements(roots);
                }
                return _menu;
            }
        }

        public void Initialize()
        {
            var id = 1;
            var groups = from c in _document.Descendants("group") select c;
            foreach (var xGroup in groups)
            {
                var @group = GetNavigationGroupDto(xGroup, id);
                foreach (var xElement in xGroup.Element("elements").Elements("element"))
                {
                    var element = GetNavigationElementDto(xElement, id, 0, @group);
                    _data.Add(element);
                    Traverse(xElement, @group, ref id);
                    id++;
                }
            }
        }

        private static NavigationGroupDto GetNavigationGroupDto(XElement xGroup, int counter)
        {
            var @group = new NavigationGroupDto()
            {
                Description = xGroup.EmptyIfNull("description"),
                Display = xGroup.EmptyIfNull("display"),
                Key = xGroup.EmptyIfNull("key"),
                Weight = counter * 100
            };

            if (!xGroup.Elements("icon").Any()) return @group;
            var icon = xGroup.Elements("icon").First();
            if (icon.Elements("cssclass").Any())
            {
                @group.IconCssClass = icon.Elements("cssclass").Single().Value;
            }
            return @group;
        }

        private static NavigationElementDto GetNavigationElementDto(XElement xElement, int id, int parentId, NavigationGroupDto @group)
        {
            var element = new NavigationElementDto()
            {
                Display = xElement.EmptyIfNull("display"),
                Id = id,
                Href = xElement.EmptyIfNull("href"),
                Group = @group,
                ParentId = parentId,
                Weight = id * 10,
            };
            if (xElement.Descendants("icon").Any())
            {
                var icon = xElement.Elements("icon").First();
                if (icon.Elements("cssclass").Any())
                {
                    element.IconCssClass = icon.Elements("cssclass").Single().Value;
                }
            }
            var role = NavigationElementRole.Anchor;
            Enum.TryParse(xElement.EmptyIfNull("role"), true, out role);
            element.Role = role;
            return element;
        }

        protected virtual void Traverse(XContainer xElement, NavigationGroupDto @group, ref int seq)
        {
            if (xElement.Element("elements") == null || !xElement.Element("elements").Elements("element").Any()) return;
            var parentId = seq;

            foreach (var descendant in xElement.Element("elements").Elements("element"))
            {
                seq++;
                var element = GetNavigationElementDto(descendant, seq, parentId, @group);

                //if (descendant.Descendants("icon").Any())
                //{
                //    var icon = descendant.Elements("icon").First();
                //    if (icon.Elements("cssclass").Any())
                //    {
                //        element.IconCssClass = icon.Elements("cssclass").Single().Value;
                //    }
                //}
                _data.Add(element);
                Traverse(descendant, @group, ref seq);
            }
        }

        public static IBackOfficeMenuProvider FromInternalConfig()
        {
            return new XmlBackOfficeMenuProvider(EmbededContent);
        }

        private static string EmbededContent
        {
            get
            {
                var content = string.Empty;

                using (var stream = Assembly.GetExecutingAssembly().
                    GetManifestResourceStream(
                        typeof(XmlBackOfficeMenuProvider).Assembly.GetManifestResourceNames()
                            .Single(x => x.EndsWith(".compiler.resources.MainMenu.xml"))))

                {
                    if (stream == null) throw new ArgumentNullException(@"Main Menu xml file", "no menu xml embeded in assembly");
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }
                return content;
            }
        }

        public static IBackOfficeMenuProvider FromConfig(string xmlContentOrPathToFile)
        {
            string content;
            try
            {
                content = File.ReadAllText(xmlContentOrPathToFile);
            }
            catch (FileNotFoundException)
            {
                content = xmlContentOrPathToFile;
            }

            return new XmlBackOfficeMenuProvider(content);
        }
    }

    internal static class Ext
    {
        public static string EmptyIfNull(this XElement element, string attrName)
        {
            return element.Attributes(attrName).Any() ? element.Attribute(attrName).Value : string.Empty;
        }
    }
}