using System;
using System.Linq;

namespace Ubik.Cache
{
    public class KeyAdjusterProviderBase
    {
        private readonly Type _type; 

        protected KeyAdjusterProviderBase(Type TEnum)
        {
            _type = TEnum;
        }

        public virtual string CalculateKey<TEnum>(TEnum t, params string[] values) where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            string prefix = CalculatePrefix(t);
            string body = string.Empty;
            if (values != null && values.Any())
            {
                body = string.Join("_", values.Select(x => x.ToLower()).ToArray());
            }
            return string.Format("{0}{1}", prefix, body);
        }

        public virtual string CalculatePrefix<TEnum>(TEnum t) where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            ThrowOnNotEnum(t);
            return string.Format("{0}_", t.ToString().ToLowerInvariant());
        }

        public virtual Type Flavor
        {
            get { return _type; }
        }

        private void ThrowOnNotEnum<TEnum>(TEnum t) where TEnum : struct, IConvertible, IComparable, IFormattable
        {
#if DNX451
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enum.");
            }
#endif
        }
    }
}