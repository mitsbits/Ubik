using System;

namespace Ubik.Infra.Contracts
{
    public interface IKeyAdjusterProvider
    {
        string AdjustKey<TEnum>(TEnum t, params string[] values) where TEnum : struct, IConvertible, IComparable, IFormattable;

        string AdjustPrefix<TEnum>(TEnum t) where TEnum : struct, IConvertible, IComparable, IFormattable;

        Type Flavor { get; }
    }
}