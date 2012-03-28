using System.Collections.Generic;
using System.Linq;

namespace Simple.Validation
{
    public class DefaultValidatorProvider : IValidatorProvider
    {
        readonly List<object> _validators = new List<object>(); 

        public void RegisterValidator<T>(IValidator<T> validator)
        {
            _validators.Add(validator);
        }

        public IEnumerable<IValidator<T>> GetValidators<T>()
        {
            return _validators.OfType<IValidator<T>>();
        }
    }

}