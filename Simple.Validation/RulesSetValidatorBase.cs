using System.Collections.Generic;
using System.Linq;

namespace Simple.Validation
{
    public abstract class RulesSetValidatorBase<T> : IValidator<T>
    {
        public string[] RulesSets { get; set; }

        public bool AppliesTo(string rulesSet)
        {
            if (string.IsNullOrWhiteSpace(rulesSet) && !this.AreRulesSetsDefined())
                return true;

            if (!this.AreRulesSetsDefined())
                return false;

            return this.RulesSets.Contains(rulesSet);
        }

        private bool AreRulesSetsDefined()
        {
            return this.RulesSets != null && this.RulesSets.Any();
        }

        public abstract IEnumerable<ValidationResult> Validate(T value);

    }
}
