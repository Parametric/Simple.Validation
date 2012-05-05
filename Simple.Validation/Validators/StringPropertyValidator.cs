using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Simple.Validation.Validators
{
    public class StringPropertyValidator<T> : PropertyValidator<T, string>
    {
        private bool _ignoreWhiteSpace;

        public override bool AppliesTo(string rulesSet)
        {
            return true;
        }

        public StringPropertyValidator(Expression<Func<T, string>> propertyExpression) : base(propertyExpression)
        {
        }

        public override IEnumerable<ValidationResult> Validate(T value)
        {
            var propertyValue = GetPropertyValue(value);
            var results = base.Validate(value, propertyValue);
            return results;
        }

        protected override string GetPropertyValue(T context)
        {
            var baseResult = base.GetPropertyValue(context);
            var result = GetValueToValidate(baseResult);
            return result;
        }

        internal string GetValueToValidate(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                if (_ignoreWhiteSpace)
                    return null;

            if (_ignoreWhiteSpace)
                return value.Trim();

            return value;
        }

        protected override bool IsSpecified(string value)
        {
            return !String.IsNullOrWhiteSpace(value);
        }

        public StringPropertyValidator<T> IgnoreWhiteSpace()
        {
            this._ignoreWhiteSpace = true;
            return this;
        }

        public StringPropertyValidator<T> DoNotIgnoreWhiteSpace()
        {
            this._ignoreWhiteSpace = false;
            return this;
        }

        public StringPropertyValidator<T> Required()
        {
            base.Required();
            return this;
        }

        public StringPropertyValidator<T> NotRequired()
        {
            base.NotRequired();
            return this;
        }

        public StringPropertyValidator<T> Severity(ValidationResultSeverity severity)
        {
            base.Severity(severity);
            return this;
        }

        public StringPropertyValidator<T> Assert(Func<T, string, bool> assertion)
        {
            base.Assert(assertion);
            return this;
        }

        public StringPropertyValidator<T> Message(string format, params object[] arguments)
        {
            base.Message(format, arguments);
            return this;
        }

        public StringPropertyValidator<T> Type(object type)
        {
            base.Type(type);
            return this;
        }

        public StringPropertyValidator<T> If(Func<T, bool> predicate)
        {
            base.If(predicate);
            return this;
        }

        public StringPropertyValidator<T> IsTrue(Func<string, bool> predicate)
        {
            base.Assert((t, s) => predicate(s));
            return this;
        }

        public StringPropertyValidator<T> IsFalse(Func<string, bool> predicate)
        {
            base.Assert((t, s) => !predicate(s));
            return this;
        }
    }
}