using System;
using System.Collections.Generic;
using System.Linq;

namespace Ubik.Infra.DataManagement.Group
{
    internal class GroupingHelper<TSource, TValue> : IGroupingService<TSource, TValue>,
        IGroupValidatorService<TSource, TValue>
    {
        private readonly Func<TSource, dynamic> _groupBy;
        private readonly Func<IEnumerable<TSource>, TValue> _groupValue;
        private readonly Func<TValue, bool> _validator;

        private IEnumerable<GroupElement<TValue>> _invalidGroups;
        private IDictionary<int, TSource> _invalidItems;

        private bool _validationHasRun = false;

        internal GroupingHelper(Func<TSource, dynamic> groupBy, Func<IEnumerable<TSource>, TValue> groupValue)
        {
            _groupBy = groupBy;
            _groupValue = groupValue;
        }

        internal GroupingHelper(IEnumerable<TSource> collection, Func<TSource, dynamic> groupBy, Func<IEnumerable<TSource>, TValue> groupValue,
            Func<TValue, bool> validator)
            : this(groupBy, groupValue)
        {
            _validator = validator;
            RunValidation(collection);
        }

        public void RunValidation(IEnumerable<TSource> source)
        {
            if (Validator == null || _validationHasRun) return;
            var collection = source.ToArray();
            WireUpValidation(GetGroups(collection, GroupBy, CalculateValue), collection);
            _validationHasRun = true;
        }

        public Func<TSource, dynamic> GroupBy
        {
            get { return _groupBy; }
        }

        public Func<IEnumerable<TSource>, TValue> CalculateValue
        {
            get { return _groupValue; }
        }

        public Func<TValue, bool> Validator
        {
            get { return _validator; }
        }

        public virtual IDictionary<object, TValue> Group(IEnumerable<TSource> collection)
        {
            var q = GetGroups(collection, GroupBy, CalculateValue);
            return q.ToDictionary(x => x.Key, x => x.Value);
        }

        public virtual bool Validate(TSource item)
        {
            if (_invalidGroups == null || !_invalidGroups.Any()) return true;
            var wrapper = WrapItem(item);
            return !_invalidGroups.Any(x => wrapper.IsInSameGroup(x.Key));
        }

        public static IGroupingService<TSource, TValue> CreateGroupService(Func<TSource, dynamic> groupBy,
        Func<IEnumerable<TSource>, TValue> groupValue)
        {
            return new GroupingHelper<TSource, TValue>(groupBy, groupValue);
        }

        public static IGroupValidatorService<TSource, TValue> CreateGroupValidationService(IEnumerable<TSource> source, Func<TSource, dynamic> groupBy,
            Func<IEnumerable<TSource>, TValue> groupValue, Func<TValue, bool> isValid)
        {
            return new GroupingHelper<TSource, TValue>(source, groupBy, groupValue, isValid);
        }

        private GroupElement<TValue> WrapItem(TSource item)
        {
            var wrapper = new GroupElement<TValue>(GroupBy.Invoke(item), CalculateValue.Invoke(new[] { item }));
            return wrapper;
        }

        private static IEnumerable<GroupElement<TValue>> GetGroups(IEnumerable<TSource> collection, Func<TSource, dynamic> groupBy, Func<IEnumerable<TSource>, TValue> getValue)
        {
            return
                collection.GroupBy(groupBy)
                    .Select(g => new GroupElement<TValue>(g.Key, getValue.Invoke(g)));
        }

        protected void WireUpValidation(IEnumerable<GroupElement<TValue>> wrappers, TSource[] collection)
        {
            PopulateInvalidGroups(wrappers);
            PopulateInvalidItems(collection);
        }

        private void PopulateInvalidGroups(IEnumerable<GroupElement<TValue>> wrappers)
        {
            _invalidGroups = new HashSet<GroupElement<TValue>>(wrappers.Where(x => !Validator.Invoke(x.Value)).ToList());
        }

        private void PopulateInvalidItems(TSource[] collection)
        {
            for (var i = 0; i < collection.Count(); i++)
            {
                var item = collection[i];
                if (!Validate(item))
                {
                    if (_invalidItems == null)
                    {
                        _invalidItems = new Dictionary<int, TSource>() { { i, item } };
                    }
                    else
                    {
                        _invalidItems.Add(new KeyValuePair<int, TSource>(i, item));
                    }
                }
            }
        }
    }
}