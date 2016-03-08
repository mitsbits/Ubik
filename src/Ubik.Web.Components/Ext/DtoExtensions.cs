using System;
using System.Collections.Generic;
using System.Linq;
using Ubik.Web.Components.DTO;

namespace Ubik.Web.Components.Ext
{
    internal static class DtoExtensions
    {
        public static object ValueFromHint(this Catalogued item)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (string.IsNullOrWhiteSpace(item.Hint)) item.Hint = HintDataType.Object.ToString();
            if (HintDataType.GetMembers().Select(x => x.ToString()).Contains (item.Hint))
            {
                var type = HintDataType.Parse(item.Hint);
                if (type.Equals(HintDataType.Boolean))
                {
                    return bool.Parse(item.Value);
                }
                if (type.Equals(HintDataType.DateTime))
                {
                    return DateTime.Parse(item.Value);
                }
                if (type.Equals(HintDataType.DateTime))
                {
                    return DateTime.Parse(item.Value);
                }
                if (type.Equals(HintDataType.Double))
                {
                    return double.Parse(item.Value);
                }
                if (type.Equals(HintDataType.Int))
                {
                    return int.Parse(item.Value);
                }
                if (type.Equals(HintDataType.Long))
                {
                    return long.Parse(item.Value);
                }
                if (type.Equals(HintDataType.Object))
                {
                    return item.Value;
                }
                if (type.Equals(HintDataType.Short))
                {
                    return short.Parse(item.Value);
                }
                if (type.Equals(HintDataType.String))
                {
                    return item.Value;
                }
                if (type.Equals(HintDataType.Uri))
                {
                    return new Uri(item.Value);
                }
            }

            return null;
        }

        public static object[] ValuesFromHint(this IEnumerable<Tiding> collection)
        {
            var result =
                collection.Where(x => HintDataType.GetMembers().Select(m => m.ToString()).Contains(string.IsNullOrWhiteSpace( x.Hint)?"object":x.Hint))
                    .OrderByDescending(x => x.Weight)
                    .Select(x => x.ValueFromHint())
                    .ToArray();
            return result;
        }

        public static IDictionary<string, object> ToDictionary(this IEnumerable<Tiding> collection)
        {
            return collection.ToDictionary(x => x.Key, x => x.Value as object);
        }
    }
}