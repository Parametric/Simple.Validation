using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Simple.Validation.Validators
{
    public class ReferencePropertyValidator<T> : PropertyValidator<T, object> 
    {
        private Type PropertyType { get; set; }
        private bool _cascade;
        private string[] RulesSets { get; set; }

        public ReferencePropertyValidator(Expression<Func<T, object>> propertyExpression)
            : base(propertyExpression)
        {
            Severity(ValidationResultSeverity.Error);
        }

        public override bool AppliesTo(string rulesSet)
        {
            return true;
        }

        public ReferencePropertyValidator<T> Cascade(params string[] rulesSets)
        {
            this.PropertyType = PropertyInfo.PropertyType;
            this._cascade = true;
            this.RulesSets = rulesSets;
            return this;
        }

        public ReferencePropertyValidator<T> Cascade<TPropertyType>(params string[] rulesSets)
        {
            if (!typeof(TPropertyType).IsAssignableFrom(PropertyInfo.PropertyType))
            {
                var msg = string.Format("PropertyType from property expression '{0}' must be convertible to '{1}'",
                    PropertyInfo.PropertyType, typeof(TPropertyType).FullName);
                throw new ArgumentOutOfRangeException(msg);
            }

            this.PropertyType = typeof(TPropertyType);
            this._cascade = true;
            this.RulesSets = rulesSets;
            return this;
        }

        public ReferencePropertyValidator<T> DoNotCascade()
        {
            this._cascade = false;
            return this;
        }

        public new ReferencePropertyValidator<T> Required()
        {
            base.Required();
            return this;
        }

        public new ReferencePropertyValidator<T> NotRequired()
        {
            base.NotRequired();
            return this;
        } 

        public new ReferencePropertyValidator<T> Message(string format, params object[] arguments)
        {
            base.Message(format, arguments);
            return this;
        }

        public new ReferencePropertyValidator<T> Severity(ValidationResultSeverity severity)
        {
            base.Severity(severity);
            return this;
        } 

        public new ReferencePropertyValidator<T> Type(object type)
        {
            base.Type(type);
            return this;
        }

        public new ReferencePropertyValidator<T> If(Func<T, bool> predicate)
        {
            base.If(predicate);
            return this;
        }

        public override IEnumerable<ValidationResult> Validate(T value)
        {
            var results = base.Validate(value).ToList();
            if (!CanValidate(value))
                return results;

            var propertyValue = base.GetPropertyValue(value);

            if (_cascade && propertyValue !=null)
                PerformCascadeValidate(value, results, propertyValue);

            return results;
        }

        private void PerformCascadeValidate(T valueBeingValidated, List<ValidationResult> results, object propertyValue)
        {
            var delegateResults = Validator
                .Validate(this.PropertyType, propertyValue, this.RulesSets)
                .ToList();
            delegateResults
                .ToList()
                .ForEach(vr =>
                             {
                                 UpdatePropertyName(vr);
                                 vr.Context = valueBeingValidated;
                             });
            results.AddRange(delegateResults);
        }

        private void UpdatePropertyName(ValidationResult vr)
        {
            vr.PropertyName = !string.IsNullOrWhiteSpace(vr.PropertyName) ? 
                string.Format("{0}.{1}", PropertyInfo.Name, vr.PropertyName) : 
                PropertyInfo.Name;
        }

    }
}
