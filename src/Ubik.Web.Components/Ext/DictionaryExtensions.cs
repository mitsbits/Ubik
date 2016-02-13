using System.Collections.Generic;

namespace Ubik.Web.Components.Ext
{
    internal static class DictionaryExtensions
    {
        #region Merge Operations

        public static void AppendOnly<TKey, TValue>(this IDictionary<TKey, TValue> home, IDictionary<TKey, TValue> data)
        {
            home.Merge(data, MergeOperation.AppendOnly);
        }

        public static void AppendAndUpdate<TKey, TValue>(this IDictionary<TKey, TValue> home, IDictionary<TKey, TValue> data)
        {
            home.Merge(data, MergeOperation.AppendAndUpdate);
        }

        public static void ReplaceAll<TKey, TValue>(this IDictionary<TKey, TValue> home, IDictionary<TKey, TValue> data)
        {
            home.Merge(data, MergeOperation.ReplaceAll);
        }

        private static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> home, IDictionary<TKey, TValue> data, MergeOperation operation = MergeOperation.AppendAndUpdate)
        {
            if (operation == MergeOperation.ReplaceAll) home.Clear();
            foreach (var key in data.Keys)
            {
                if (home.ContainsKey(key) && operation == MergeOperation.AppendAndUpdate)
                {
                    home[key] = data[key];
                }
                else
                {
                    if (!home.Keys.Contains(key))
                        home.Add(key, data[key]);
                }
            }
        }

        private enum MergeOperation
        {
            AppendAndUpdate = 1,
            AppendOnly = 2,
            ReplaceAll = 3
        }

        #endregion Merge Operations
    }
}