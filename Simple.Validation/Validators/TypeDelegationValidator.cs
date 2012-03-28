using System;
using System.Collections.Generic;

namespace Simple.Validation.Validators
{
    public class TypeDelegationValidator<TSubClass, TSuperClass> : IValidator<TSubClass> where TSubClass : TSuperClass
    {

        public bool AppliesTo(string rulesSet)
        {
            return true;
        }

        public IEnumerable<ValidationResult> Validate(TSubClass value)
        {
            throw new NotImplementedException();
        }

    }
}
