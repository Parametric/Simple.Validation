using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Simple.Validation.Comparers;

namespace Simple.Validation.Validators
{
    public class EnumerablePropertyValidator<T> : PropertyValidatorBase<T>
    {
        private bool _required;
        private ValidationResultSeverity _severity = ValidationResultSeverity.Error;
        private string _message;
        private object _type;
        private int? _minSize;
        private int? _maxSize;
        private bool _unique;
        private IEqualityComparer<object> _uniqueComparer;

        public EnumerablePropertyValidator(LambdaExpression propertyExpression) : base(propertyExpression)
        {
        }

        public override bool AppliesTo(string rulesSet)
        {
            return true;
        }

        public override IEnumerable<ValidationResult> Validate(T value)
        {
            var enumerableValue = GetPropertyValue<IEnumerable<object>>(value);
            if (_required && enumerableValue == null)
                yield return NewValidationResult(value);

            if (enumerableValue == null)
                yield break;

            var size = enumerableValue.Count();
            if (_minSize.HasValue && _minSize > size) 
                yield return NewValidationResult(value);

            if (_maxSize.HasValue && _maxSize < size)
                yield return NewValidationResult(value);

            if (_unique)
            {
                var uniqueValidationResult = ValidateUnique(value, enumerableValue);
                if (uniqueValidationResult != null)
                    yield return uniqueValidationResult;
            }
        }

        private ValidationResult ValidateUnique(T context, IEnumerable<object> enumerable)
        {
            if (!_unique)
                return null;

            var isUnique = IsDistinct(enumerable);
            if (isUnique)
                return null;

            return NewValidationResult(context);
        }

        private bool IsDistinct(IEnumerable<object> enumerable)
        {
            bool result;
            if (_uniqueComparer == null)
                result = enumerable.Count() == enumerable.Distinct().Count();
            else
                result = enumerable.Count() == enumerable.Distinct(_uniqueComparer).Count();
            return result;
        }

        private ValidationResult NewValidationResult(T value)
        {
            return new ValidationResult()
                       {
                           Severity = _severity,
                           Message = _message,
                           PropertyName = PropertyInfo.Name,
                           Context = value,
                           Type = _type
                       };
        }

        public EnumerablePropertyValidator<T> Required()
        {
            _required = true;
            return this;
        }

        public EnumerablePropertyValidator<T> NotRequired()
        {
            _required = false;
            return this;
        }

        public EnumerablePropertyValidator<T> Size(int? minSize, int? maxSize = null)
        {
            _minSize = minSize;
            _maxSize = maxSize;
            return this;
        }

        public EnumerablePropertyValidator<T> Unique()
        {
            _unique = true;
            return this;
        }

        public EnumerablePropertyValidator<T> Unique<TPropertyType>(IEqualityComparer<TPropertyType> equalityComparer)
        {
            var equalityComparerAdapter = new EqualityComparerAdapter<T, TPropertyType>(equalityComparer);
            return Unique()
                .UsingUniqueComparer(equalityComparerAdapter);
        }

        public EnumerablePropertyValidator<T> Unique<TPropertyType>(Func<TPropertyType, TPropertyType, bool> predicate )
        {
            var internalComparer = new PredicateEqualityComparer<TPropertyType>(predicate);
            return Unique()
                .UsingUniqueComparer(internalComparer);
        }

        public EnumerablePropertyValidator<T> Unique<TSource>(Func<TSource, IComparable> selector)
        {
            var internalComparer = new SelectorEqualityComparer<TSource>(selector);
            return Unique().UsingUniqueComparer(internalComparer);
        }

        public EnumerablePropertyValidator<T> UsingUniqueComparer<TPropertyType>(IEqualityComparer<TPropertyType> comparer )
        {
            var adapter = new EqualityComparerAdapter<T, TPropertyType>(comparer);
            return UsingUniqueComparer(adapter);
        } 

        public EnumerablePropertyValidator<T> UsingUniqueComparer(IEqualityComparer<object> equalityComparer)
        {
            _uniqueComparer = equalityComparer;
            return this;
        }
    }
}
