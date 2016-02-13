using System;

namespace Ubik.Infra.Contracts
{
    public interface ICacheProvider
    {
        /// <summary>
        /// Gets an item from the storage.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        object GetItem(string key);

        /// <summary>
        /// Stores an item to storage with the maximum expiration date.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void SetItem(string key, object value);

        /// <summary>
        /// Stores an item to storage and sets it life span to the value of minutes provided.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="ttl">The time fragments expected seconds, minutes, hours, days</param>
        void SetItem(string key, object value, params int[] ttl);

        /// <summary>
        /// Stores an item to storage and sets it life span to the value of minutes provided.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="absoluteExpiration"> A point in time that after that the cache item is invalid</param>
        void SetItem(string key, object value, DateTime absoluteExpiration);

        /// <summary>
        /// Removes the item from the storage.
        /// </summary>
        /// <param name="key">The key.</param>
        void RemoveItem(string key);
    }
}