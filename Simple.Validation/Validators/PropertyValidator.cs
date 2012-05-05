using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Simple.Validation.Validators
{
    public abstract class PropertyValidator<TContext, TProperty> : IValidator<TContext>
    {
        private ValidationResultSeverity _severity;
        private string _message;
        private bool _required;
        private object _type;

        private Func<TContext, bool> _predicate;

        protected List<Func<TContext, TProperty, bool>> Assertions { get; private set; }

        protected PropertyValidator(LambdaExpression  propertyExpression)
        {
            PropertyExpression = propertyExpression;
            PropertyInfo = Expressions.GetPropertyInfoFromExpression(propertyExpression);
        
            Assertions = new List<Func<TContext, TProperty, bool>>();
            Severity(ValidationResultSeverity.Error);

        }

        protected LambdaExpression  PropertyExpression { get; private set; }

        protected PropertyInfo PropertyInfo { get; private set; }

        protected virtual TProperty GetPropertyValue(TContext context)
        {
            return (TProperty)PropertyExpression.Compile().DynamicInvoke(context);
        }

        protected IEnumerable<ValidationResult> Validate(TContext context, TProperty value)
        {
            if (!CanValidate(context))
                yield break;

            if (_required && !IsSpecified(value))
                yield return NewValidationResult(context, _message, TextValidationResultType.RequiredValueNotFound);

            if (value == null)
                yield break;

            if (Assertions.Any(f => f.Invoke(context, value) == false))
                yield return NewValidationResult(context, _message, null);
        }

        protected ValidationResult NewValidationResult(object context)
        {
            return new ValidationResult()
            {
                Context = context,
                Message = _message,
                PropertyName = PropertyInfo.Name,
                Type = _type ,
                Severity = _severity,
            };
        }

        protected ValidationResult NewValidationResult(object context, string message, object type)
        {
            return new ValidationResult()
            {
                Context = context,
                Message = message,
                PropertyName = PropertyInfo.Name,
                Type = _type ?? type,
                Severity = _severity,
            };
        }

        protected virtual bool IsSpecified(TProperty value)
        {
            return value != null;
        }

        protected bool CanValidate(TContext context)
        {
            return _predicate == null || _predicate(context);
        }

        public abstract bool AppliesTo(string rulesSet);

        public virtual IEnumerable<ValidationResult> Validate(TContext value)
        {
            var propertyValue = GetPropertyValue(value);
            var results = Validate(value, propertyValue);
            return results;
        }

        protected void SetRequired(bool value)
        {
            _required = value;
        }

        protected void SetSeverity(ValidationResultSeverity severity)
        {
            _severity = severity;
        }

        protected void SetMessage(string format, params object[] arguments)
        {
            _message = string.Format(format, arguments);
        }

        protected void SetType(object type)
        {
            _type = type;
        }

        protected void SetPredicate(Func<TContext, bool> predicate)
        {
            _predicate = predicate;
        }









        public PropertyValidator<TContext, TProperty> Required()
        {
            SetRequired(true);
            return this;
        }

        public PropertyValidator<TContext, TProperty> NotRequired()
        {
            SetRequired(false);
            return this;
        }

        public PropertyValidator<TContext, TProperty> Severity(ValidationResultSeverity severity)
        {
            SetSeverity(severity);
            return this;
        }

        public PropertyValidator<TContext, TProperty> Assert(Func<TContext, TProperty, bool> assertion)
        {
            Assertions.Add(assertion);
            return this;
        }

        public PropertyValidator<TContext, TProperty> Message(string format, params object[] arguments)
        {
            SetMessage(format, arguments);
            return this;
        }

        public PropertyValidator<TContext, TProperty> Type(object type)
        {
            SetType(type);
            return this;
        }

        public PropertyValidator<TContext, TProperty> If(Func<TContext, bool> predicate)
        {
            SetPredicate(predicate);
            return this;
        }
    }
}