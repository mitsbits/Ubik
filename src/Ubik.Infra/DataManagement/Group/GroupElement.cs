using System.Collections.Generic;
using System.Reflection;

namespace Ubik.Infra.DataManagement.Group
{
    internal class GroupElement<TValue>
    {
        private readonly dynamic _source;
        private readonly TValue _value;

        internal GroupElement(dynamic d, TValue value)
        {
            _source = d;
            _value = value;
        }

        public dynamic Key
        {
            get { return _source; }
        }

        public TValue Value
        {
            get { return _value; }
        }

        private IEnumerable<PropertyInfo> Properties
        {
            get
            {
                return _source.GetType().GetProperties();
                ;
            }
        }

        public bool IsInSameGroup(dynamic target)
        {
            var targetType = target.GetType();
            var result = false;
            foreach (var propertyInfo in Properties)
            {
                var property = targetType.GetProperty(propertyInfo.Name);
                result = property.GetValue(target, null).Equals(propertyInfo.GetValue(_source));
                if (!result) break;
            }
            return result;
        }
    }
}