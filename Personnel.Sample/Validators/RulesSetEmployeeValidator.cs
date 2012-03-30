using System.Collections.Generic;
using System.Linq;
using Simple.Validation;

namespace Personnel.Sample.Validators
{
    public class RulesSetEmployeeValidator : RulesSetValidatorBase<Employee>
    {
        public override IEnumerable<ValidationResult> Validate(Employee value)
        {
            var firstNameResults = Properties<Employee>
                .For(e => e.FirstName)
                .Length(3, 50)
                .Required()
                .IgnoreWhiteSpace()
                .Validate(value);

            var lastNameResults = Properties<Employee>
                .For(e => e.LastName)
                .Length(3, 50)
                .Required()
                .IgnoreWhiteSpace()
                .Validate(value);

            var ageResults = Properties<Employee>
                .For(e => e.Age)
                .MinValue(18)
                .MaxValue(35)
                .Validate(value);

            return firstNameResults
                .Concat(lastNameResults)
                .Concat(ageResults)
                ;
        }

        public RulesSetEmployeeValidator()
        {
            RulesSets = new[]{Sample.RulesSets.Crud.Save};
        }
    }
}
