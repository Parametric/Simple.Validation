using System.Collections.Generic;
using System.Linq;
using Simple.Validation;

namespace Personnel.Sample.Validators
{
    public class EmployeeInterfaceValidator : IValidator<IEmployee>
    {
        public bool AppliesTo(string rulesSet)
        {
            return rulesSet == EmployeeOperations.CreateNewEmployee;
        }

        public IEnumerable<ValidationResult> Validate(IEmployee value)
        {
            var firstNameResults = Properties<IEmployee>
                .For(e => e.FirstName)
                .Length(3, 50)
                .Required()
                .IgnoreWhiteSpace()
                .Validate(value);

            var lastNameResults = Properties<IEmployee>
                .For(e => e.LastName)
                .Length(3, 50)
                .Required()
                .IgnoreWhiteSpace()
                .Validate(value);

            var ageResults = Properties<IEmployee>
                .For(e => e.Age)
                .MinValue(18)
                .MaxValue(35)
                .Validate(value);


            return firstNameResults
                .Concat(lastNameResults)
                .Concat(ageResults)
                ;
        }
    }
}
