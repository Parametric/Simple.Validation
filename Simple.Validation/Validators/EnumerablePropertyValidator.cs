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

        protected PropertyValidator<T, IEnumerable<object>> AsBaseType()
        {
            return this;
        } 

        public EnumerablePropertyValidator<T> Unique()
        {
            AsBaseType().Unique();
            return this;
        }

        public EnumerablePropertyValidator<T> Unique<TElement>(IEqualityComparer<TElement> comparer)
        {
            var adapter = new EqualityComparerAdapter<T, TElement>(comparer);
            AsBaseType().Unique(adapter);
            return this;
        }

        public EnumerablePropertyValidator<T> Unique<TElement>(Func<TElement, TElement, bool> predicate )
        {
            var comparer = new PredicateEqualityComparer<TElement>(predicate);
            return this.Unique(comparer);
        }

        public EnumerablePropertyValidator<T> Unique<TElementType>(Func<TElementType, IComparable> selector)
        {
            var comparer = new SelectorEqualityComparer<TElementType>(selector);
            return this.Unique(comparer);
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

        public new EnumerablePropertyValidator<T> Severity(ValidationResultSeverity severity)
        {
            base.Severity(severity);
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
