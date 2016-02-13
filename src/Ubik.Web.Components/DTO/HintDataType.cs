using System;
using System.Collections.Generic;

namespace Ubik.Web.Components.DTO
{
    internal class HintDataType
    {
        private readonly string _typeDescription = string.Empty;
        private static readonly IDictionary<string, HintDataType> Dict = new Dictionary<string, HintDataType>();

        public static readonly HintDataType String = new HintDataType("string");
        public static readonly HintDataType Short = new HintDataType("short");
        public static readonly HintDataType Int = new HintDataType("int");
        public static readonly HintDataType Long = new HintDataType("long");
        public static readonly HintDataType Double = new HintDataType("double");
        public static readonly HintDataType Object = new HintDataType("object");
        public static readonly HintDataType DateTime = new HintDataType("dateTime");
        public static readonly HintDataType Boolean = new HintDataType("boolean");
        public static readonly HintDataType Uri = new HintDataType("uri");

        private HintDataType(string typeDescription)
        {
            _typeDescription = typeDescription;
            Dict.Add(typeDescription, this);
        }

        public override string ToString()
        {
            return _typeDescription;
        }

        public static HintDataType Parse(string typeDescription)
        {
            if (Dict.Keys.Contains(typeDescription))
            {
                return Dict[typeDescription];
            }
            throw new NotImplementedException("This type description is not supported currently.");
        }

        public static bool TryParse(string typeDescription, out HintDataType type)
        {
            try
            {
                type = Parse(typeDescription);
                return true;
            }
            catch (NotImplementedException)
            {
                type = null;
                return false;
            }
        }

        public static List<HintDataType> GetMembers()
        {
            return new List<HintDataType>(Dict.Values);
        }
    }
}