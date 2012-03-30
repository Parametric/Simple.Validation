using System.Collections.Generic;
using System.Linq;

namespace Simple.Validation.Validators
{
    public abstract class CompositeValidator<T> : IValidator<T>
    {
        public abstract bool AppliesTo(string rulesSet);

        public IEnumerable<ValidationResult> Validate(T value)
        {
            var propertyValidators = GetInternalValidators();
            var results = propertyValidators.SelectMany(v => v.Validate(value));
            return results;
        }

        protected abstract IEnumerable<IValidator<T>> GetInternalValidators();
    }
}
