using System;
using System.Collections.Generic;
using System.Linq;

namespace Ubik.Web.Components.DTO
{
  
    public class Tidings : ICollection<Tiding>, IDictionary<string, object>
    {
        private readonly ICollection<Tiding> _bucket;

        public Tidings()
        {
            _bucket = new HashSet<Tiding>();
        }

        #region ICollection

        public void Add(Tiding item)
        {
            _bucket.Add(item);
        }

        public void Clear()
        {
            _bucket.Clear();
        }

        public bool Contains(Tiding item)
        {
            return _bucket.Contains(item);
        }

        public void CopyTo(Tiding[] array, int arrayIndex)
        {
            _bucket.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _bucket.Count; }
        }

        public bool IsReadOnly
        {
            get { return _bucket.IsReadOnly; }
        }

        public bool Remove(Tiding item)
        {
            if (!_bucket.Contains(item)) return false;
            _bucket.Remove(item);
            return true;
        }

        public IEnumerator<Tiding> GetEnumerator()
        {
            return _bucket.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _bucket.GetEnumerator();
        }

        #endregion ICollection

        #region IDictionary

        public void Add(string key, object value)
        {
            if (_bucket.Any(x => x.Key == key))
            {
                _bucket.Single(x => x.Key == key).Value = value.ToString();
            }
            else
            {
                _bucket.Add(new Tiding() { Key = key, Value = value.ToString() });
            }
        }

        public bool ContainsKey(string key)
        {
            return _bucket.Any(x => x.Key == key);
        }

        public ICollection<string> Keys
        {
            get { return _bucket.Select(x => x.Key).Distinct().ToList(); }
        }

        public bool Remove(string key)
        {
            var hit = _bucket.FirstOrDefault(x => x.Key == key);
            return hit != null && _bucket.Remove(hit);
        }

        public bool TryGetValue(string key, out object value)
        {
            var hit = _bucket.FirstOrDefault(x => x.Key == key);
            if (hit == null)
            {
                value = null;
                return false;
            }
            value = hit.Value;
            return true;
        }

        public ICollection<object> Values
        {
            get { return _bucket.Select(x => x.Value as object).ToList(); }
        }

        public object this[string key]
        {
            get
            {
                var hit = _bucket.FirstOrDefault(x => x.Key == key);
                return hit != null ? hit.Value : string.Empty;
            }
            set
            {
                var hit = _bucket.FirstOrDefault(x => x.Key == key);
                if (hit != null)
                {
                    hit.Value = value.ToString();
                }
                else
                {
                    _bucket.Add(new Tiding() { Key = key, Value = value.ToString() });
                }
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            if (_bucket.Any(x => x.Key == item.Key))
            {
                _bucket.Single(x => x.Key == item.Key).Value = item.Value.ToString();
            }
            else
            {
                _bucket.Add(new Tiding() { Key = item.Key, Value = item.Value.ToString() });
            }
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _bucket.Any(x => x.Key == item.Key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            var dic = from t in _bucket select new KeyValuePair<string, object>(t.Key, t.Value);
            dic.ToArray().CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            var hit = _bucket.FirstOrDefault(x => x.Key == item.Key);
            if (hit == null) return false;
            return _bucket.Remove(hit);
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            var dic = from t in _bucket select new KeyValuePair<string, object>(t.Key, t.Value);
            return dic.GetEnumerator();
        }

        #endregion IDictionary
    }
}