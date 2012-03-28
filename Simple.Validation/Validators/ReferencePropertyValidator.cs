using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Simple.Validation.Validators
{
    public class ReferencePropertyValidator<T> : IValidator<T>
    {
        private readonly Expression<Func<T, object>> _propertyExpression;
        private readonly ReferencePropertyRequirements _referencePropertyRequirements;
        private readonly string _propertyName;

        public bool AppliesTo(string rulesSet)
        {
            return true;
        }

        public ReferencePropertyValidator<T> Required()
        {
            this._referencePropertyRequirements.Required = true;
            return this;
        }

        public ReferencePropertyValidator<T> NotRequired()
        {
            this._referencePropertyRequirements.Required = false;
            return this;
        } 

        public ReferencePropertyValidator<T> Cascade(params string[] rulesSets)
        {
            this._referencePropertyRequirements.Cascade = true;
            this._referencePropertyRequirements.RulesSets = rulesSets;
            return this;
        }

        public ReferencePropertyValidator<T> DoNotCascade()
        {
            this._referencePropertyRequirements.Cascade = false;
            return this;
        }

        public ReferencePropertyValidator<T> Message(string format, params object[] arguments)
        {
            _referencePropertyRequirements.Message = string.Format(format, arguments);
            return this;
        }

        public ReferencePropertyValidator<T> Severity(ValidationResultSeverity severity)
        {
            _referencePropertyRequirements.Severity = severity;
            return this;
        } 

        public ReferencePropertyValidator<T> Type(object type)
        {
            _referencePropertyRequirements.Type = type;
            return this;
        } 

        public IEnumerable<ValidationResult> Validate(T value)
        {
            var results = new List<ValidationResult>();

            var propertyValue = _propertyExpression.Compile().Invoke(value);
            CheckRequired(value, results, propertyValue);

            if (_referencePropertyRequirements.Cascade)
                PerformCascadeValidate(results, propertyValue);

            return results;
        }

        private void PerformCascadeValidate(List<ValidationResult> results, object propertyValue)
        {
            var delegateResults = Validator
                .Validate(propertyValue, _referencePropertyRequirements.RulesSets)
                .ToList();
            delegateResults
                .ToList()
                .ForEach(UpdateValidationResultPropertyName);
            results.AddRange(delegateResults);
        }

        private void UpdateValidationResultPropertyName(ValidationResult vr)
        {
            vr.PropertyName = !string.IsNullOrWhiteSpace(vr.PropertyName) ? 
                string.Format("{0}.{1}", _propertyName, vr.PropertyName) : 
                _propertyName;
        }

        private void CheckRequired(T value, List<ValidationResult> results, object propertyValue)
        {
            if (_referencePropertyRequirements.Required && propertyValue == null)
                results.Add(new ValidationResult()
                                {
                                    Context = value,
                                    Message = _referencePropertyRequirements.Message,
                                    PropertyName = _propertyName,
                                    Severity = _referencePropertyRequirements.Severity,
                                    Type = _referencePropertyRequirements.Type,
                                });
        }

        public ReferencePropertyValidator(Expression<Func<T, object>> propertyExpression)
        {
            _propertyExpression = propertyExpression;
            this._referencePropertyRequirements = new ReferencePropertyRequirements();

            var propertyInfo = Expressions.GetPropertyInfoFromExpression(propertyExpression);

            _propertyName = propertyInfo.Name;
        }
    }
}
