using System;
using System.Collections.Generic;

namespace Ubik.Infra.DataManagement.Group
{
    internal interface IGroupingService<in TSource, TValue>
    {
        Func<TSource, dynamic> GroupBy { get; }

        Func<IEnumerable<TSource>, TValue> CalculateValue { get; }

        IDictionary<object, TValue> Group(IEnumerable<TSource> collection);
    }
}