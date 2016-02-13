using System;
using System.Collections.Generic;

namespace Ubik.Web.Components
{
    public class ModuleType
    {
        private readonly string _flavorDescription = String.Empty;
        private static readonly IDictionary<string, ModuleType> Dict = new Dictionary<string, ModuleType>();

        public static readonly ModuleType Empty = new ModuleType("Empty");
        public static readonly ModuleType PartialAction = new ModuleType("Partial Action");
        public static readonly ModuleType PartialView = new ModuleType("Partial View");
        public static readonly ModuleType ViewComponent = new ModuleType("Partial View Component");

        private ModuleType(string flavorDescription)
        {
            _flavorDescription = flavorDescription;
            Dict.Add(flavorDescription, this);
        }

        public override string ToString()
        {
            return _flavorDescription;
        }

        public static ModuleType Parse(string flavorDescription)
        {
            if (Dict.Keys.Contains(flavorDescription))
            {
                return Dict[flavorDescription];
            }
            throw new NotImplementedException("This type description is not supported currently.");
        }

        public static bool TryParse(string heightDescription, out ModuleType type)
        {
            try
            {
                type = Parse(heightDescription);
                return true;
            }
            catch (NotImplementedException)
            {
                type = null;
                return false;
            }
        }

        public static List<ModuleType> GetMembers()
        {
            return new List<ModuleType>(Dict.Values);
        }
    }
}