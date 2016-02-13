using System;
using System.Collections.Generic;

namespace Ubik.Infra.Contracts
{
    public interface IKeyAdjuster
    {
        IEnumerable<IKeyAdjusterProvider> Providers { get; }

        string AdjustKey<TEnum>(TEnum t, params string[] values) where TEnum : struct, IConvertible, IComparable, IFormattable;
    }
}