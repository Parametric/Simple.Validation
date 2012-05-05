using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Simple.Validation.Comparers;

namespace Simple.Validation.Validators
{
    public class EnumerablePropertyValidator<T> : PropertyValidator<T, IEnumerable<object>>
    {

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
            var results = base.Validate(value).ToList();

            var enumerableValue = GetPropertyValue(value);

            if (!CanValidate(value))
                return results;

            if (!IsSpecified(enumerableValue))
                return results;

            if (enumerableValue == null)
                return results;

            var materialized = enumerableValue.ToList();

            //var size = materialized.Count();
            //if (_minCount.HasValue && _minCount > size) 
            //    results.Add(NewValidationResult(value));

            //if (_minSize.HasValue && _minSize < size)
            //    results.Add(NewValidationResult(value));

            //if (_unique)
            //{
            //    var uniqueValidationResult = ValidateUnique(value, materialized);
            //    if (uniqueValidationResult != null)
            //        results.Add(uniqueValidationResult);
            //}

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

        private bool IsDistinct(IEnumerable<object> enumerable, IEqualityComparer<object> comparer )
        {
            bool result;
            if (comparer == null)
                result = enumerable.Count() == enumerable.Distinct().Count();
            else
                result = enumerable.Count() == enumerable.Distinct(comparer).Count();
            return result;
        }

        public EnumerablePropertyValidator<T> Count(int? minCount, int? maxCount = null)
        {
            if (minCount.HasValue)
                Assert((t, list) => list.Count() >= minCount);

            if (maxCount.HasValue)
                Assert((t, list) => list.Count() <= maxCount);

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
            _cascadePropertyType = typeof(TPropertyType);
            _cascade = true;
            _cascadeRulesSets = rulesSets;
            return this;
        }

        public new EnumerablePropertyValidator<T> Message(string format, params object[] arguments)
        {
            base.Message(format, arguments);
            return this;
        } 

        public new EnumerablePropertyValidator<T> NotRequired()
        {
            base.NotRequired();
            return this;
        }

        public new EnumerablePropertyValidator<T> Required()
        {
            base.Required();
            return this;
        }

        public EnumerablePropertyValidator<T> Unique()
        {
            return Unique<object>((left, right) => left.Equals(right));
        }

        public EnumerablePropertyValidator<T> Unique<TElementType>(IEqualityComparer<TElementType> comparer)
        {
            Assert((t, list) =>
            {
                var adapter = new EqualityComparerAdapter<T, TElementType>(comparer);
                var result = IsDistinct(list, adapter);
                return result;
            });

            return this;
        }

        public EnumerablePropertyValidator<T> Unique<TElementType>(Func<TElementType, TElementType, bool> predicate )
        {
            var comparer = new PredicateEqualityComparer<TElementType>(predicate);
            return Unique(comparer);
        }

        public EnumerablePropertyValidator<T> Unique<TElementType>(Func<TElementType, IComparable> selector)
        {
            var comparer = new SelectorEqualityComparer<TElementType>(selector);
            return Unique(comparer);
        }

        public new EnumerablePropertyValidator<T> Severity(ValidationResultSeverity severity)
        {
            base.SetSeverity(severity);
            return this;
        }

        public new EnumerablePropertyValidator<T> Type(object customType)
        {
            base.Type(customType);
            return this;
        }

        public new EnumerablePropertyValidator<T> If(Func<T, bool> predicate)
        {
            base.If(predicate);
            return this;
        }
    }
}
