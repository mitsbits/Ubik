using System;

namespace Ubik.Infra.DataManagement.Group
{
    internal interface IGroupValidatorService<in TSource, in TValue>
    {
        Func<TValue, bool> Validator { get; }

        bool Validate(TSource item);
    }
}