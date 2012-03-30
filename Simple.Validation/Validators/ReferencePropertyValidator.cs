using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Simple.Validation.Validators
{
    public class ReferencePropertyValidator<T> : PropertyValidatorBase<T> 
    {
        protected bool _required;
        protected Type PropertyType { get; set; }
        protected bool _cascade;
        private string _message;
        private ValidationResultSeverity _severity = ValidationResultSeverity.Error;
        private object _type;
        protected string[] RulesSets { get; set; }

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

        public ReferencePropertyValidator<T> Required()
        {
            this._required = true;
            return this;
        }

        public ReferencePropertyValidator<T> NotRequired()
        {
            this._required = false;
            return this;
        } 

        public ReferencePropertyValidator<T> Message(string format, params object[] arguments)
        {
            _message = string.Format(format, arguments);
            return this;
        }

        public ReferencePropertyValidator<T> Severity(ValidationResultSeverity severity)
        {
            _severity = severity;
            return this;
        } 

        public ReferencePropertyValidator<T> Type(object type)
        {
            _type = type;
            return this;
        } 

        public override IEnumerable<ValidationResult> Validate(T value)
        {
            var results = new List<ValidationResult>();

            var propertyValue = base.GetPropertyValue<object>(value);
            CheckRequired(value, results, propertyValue);

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

        private void CheckRequired(T value, ICollection<ValidationResult> results, object propertyValue)
        {
            if (_required && propertyValue == null)
                results.Add(new ValidationResult()
                                {
                                    Context = value,
                                    Message = _message,
                                    PropertyName = PropertyInfo.Name,
                                    Severity = _severity,
                                    Type = _type,
                                });
        }

        public ReferencePropertyValidator(Expression<Func<T, object>> propertyExpression) : base(propertyExpression)
        {
            Severity(ValidationResultSeverity.Error);
        }
    }
}
