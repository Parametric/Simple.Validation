using System.Collections.Generic;

namespace Simple.Validation
{
    public interface IValidatorProvider
    {
        IEnumerable<IValidator<T>> GetValidators<T>(params string[] rulesSets);
    }
}