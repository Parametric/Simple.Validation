using System.Collections.Generic;
using System.Linq;
using Ninject;

namespace Simple.Validation.Ninject
{
    public class NinjectValidatorProvider : IValidatorProvider
    {
        private readonly IKernel _kernel;

        public IEnumerable<IValidator<T>> GetValidators<T>(params string[] rulesSets)
        {
            if (rulesSets == null || !rulesSets.Any())
                rulesSets = new[]{""};

            var query = from validator in _kernel.GetAll<IValidator<T>>()
                        from rulesSet in rulesSets
                        where validator.AppliesTo(rulesSet)
                        select validator;

            var results = query.ToList();
            return results;
        }

        public NinjectValidatorProvider(IKernel kernel)
        {
            _kernel = kernel;
        }
    }
}
