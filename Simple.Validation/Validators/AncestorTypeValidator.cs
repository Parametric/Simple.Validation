using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Validation.Validators
{
    public class AncestorTypeValidator<TSubType, TSuperType> : RulesSetValidatorBase<TSuperType> where TSubType: TSuperType
    {
        public override IEnumerable<ValidationResult> Validate(TSuperType value)
        {
            var results = Validator.Validate(typeof(TSubType), value, RulesSets);
            return results;
        }

        public AncestorTypeValidator(params string[] rulesSets)
        {
            RulesSets = rulesSets;
        }
    }
}
