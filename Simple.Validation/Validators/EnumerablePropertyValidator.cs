﻿using System;
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
        private bool _cascade;
        private string[] _cascadeRulesSets;
        private Type _cascadePropertyType;

        public EnumerablePropertyValidator(LambdaExpression propertyExpression) : base(propertyExpression)
        {
        }

        public override bool AppliesTo(string rulesSet)
        {
            return true;
        }

        public override IEnumerable<ValidationResult> Validate(T value)
        {
            var results = new List<ValidationResult>();

            var enumerableValue = GetPropertyValue<IEnumerable<object>>(value);
            if (_required && enumerableValue == null)
                results.Add(NewValidationResult(value));

            if (enumerableValue == null)
                return results;

            var materialized = enumerableValue.ToList();

            var size = materialized.Count();
            if (_minSize.HasValue && _minSize > size) 
                results.Add(NewValidationResult(value));

            if (_maxSize.HasValue && _maxSize < size)
                results.Add(NewValidationResult(value));

            if (_unique)
            {
                var uniqueValidationResult = ValidateUnique(value, materialized);
                if (uniqueValidationResult != null)
                    results.Add(uniqueValidationResult);
            }

            if (_cascade)
                CascadeValidate(value, materialized, results);

            return results;
        }

        private void CascadeValidate(T context, IEnumerable<object> enumerableValue, List<ValidationResult> results)
        {
            var list = enumerableValue.ToList();
            for (var i = 0; i < list.Count; i++)
            {
                var item = list[0];
                var typeOfValidatorToUse = _cascadePropertyType ?? item.GetType();
                var validationResults = Validator.Validate(typeOfValidatorToUse, item, _cascadeRulesSets).ToList();

                SetPropertyNamePrefix(context, validationResults, i);

                results.AddRange(validationResults);         
            }
        }

        private void SetPropertyNamePrefix(T context, IEnumerable<ValidationResult> validationResults, int i)
        {
            var prefix = string.Format("{0}[{1}]", PropertyInfo.Name, i);
            foreach (var validationResult in validationResults)
            {
                validationResult.Context = context;
                validationResult.PropertyName = string.Format("{0}.{1}", prefix, validationResult.PropertyName);
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

        public EnumerablePropertyValidator<T> Unique<TPropertyType>(IEqualityComparer<TPropertyType> comparer)
        {
            var adapter = new EqualityComparerAdapter<T, TPropertyType>(comparer);
            return Unique()
                .UsingUniqueComparer(adapter);
        }

        public EnumerablePropertyValidator<T> Unique<TPropertyType>(Func<TPropertyType, TPropertyType, bool> predicate )
        {
            var comparer = new PredicateEqualityComparer<TPropertyType>(predicate);
            return Unique()
                .UsingUniqueComparer(comparer);
        }

        public EnumerablePropertyValidator<T> Unique<TSource>(Func<TSource, IComparable> selector)
        {
            var comparer = new SelectorEqualityComparer<TSource>(selector);
            return Unique().UsingUniqueComparer(comparer);
        }

        public EnumerablePropertyValidator<T> UsingUniqueComparer<TPropertyType>(IEqualityComparer<TPropertyType> comparer )
        {
            var adapter = new EqualityComparerAdapter<T, TPropertyType>(comparer);
            return UsingUniqueComparer(adapter);
        } 

        public EnumerablePropertyValidator<T> UsingUniqueComparer(IEqualityComparer<object> comparer)
        {
            _uniqueComparer = comparer;
            return this;
        }

        public EnumerablePropertyValidator<T> Cascade(params string[] rulesSets)
        {
            _cascade = true;
            _cascadeRulesSets = rulesSets;
            return this;
        }

        public EnumerablePropertyValidator<T> Cascade<TPropertyType>(params string[] rulesSets)
        {
            _cascadePropertyType = typeof (TPropertyType);
            _cascade = true;
            _cascadeRulesSets = rulesSets;
            return this;
        }
    
        public EnumerablePropertyValidator<T> Message(string format, params object[] arguments)
        {
            _message = string.Format(format, arguments);
            return this;
        } 

        public EnumerablePropertyValidator<T> Severity(ValidationResultSeverity severity)
        {
            _severity = severity;
            return this;
        }

        public EnumerablePropertyValidator<T> Type(object customType)
        {
            _type = customType;
            return this;
        }
    }
}
