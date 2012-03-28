using System.Collections.Generic;
using System.Linq;
using Ninject;

namespace Simple.Validation.Ninject
{
    public class NinjectValidatorProvider : IValidatorProvider
    {
        private readonly IKernel _kernel;

        public IEnumerable<IValidator<T>> GetValidators<T>()
        {
            return _kernel.GetAll<IValidator<T>>();
        }

        public NinjectValidatorProvider(IKernel kernel)
        {
            _kernel = kernel;
        }
    }
}
