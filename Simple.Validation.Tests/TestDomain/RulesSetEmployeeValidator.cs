using System.Collections.Generic;
using System.Linq;

namespace Simple.Validation.Tests.TestDomain
{
    public class RulesSetEmployeeValidator : RulesSetValidatorBase<Employee>
    {

        public override IEnumerable<ValidationResult> Validate(Employee value)
        {
            var firstNameResults = StringValidator
                .Validate(new StringRequirements()
                {
                    MinLength = 3,
                    MaxLength = 50,
                    Required = true,
                    IgnoreWhiteSpace = true
                }
                          , value.FirstName
                          , "FirstName"
                          , value);

            var lastNameResults = StringValidator
                .Validate(new StringRequirements()
                {
                    MinLength = 3,
                    MaxLength = 50,
                    Required = true,
                    IgnoreWhiteSpace = true
                }
                          , value.LastName
                          , "LastName"
                          , value);

            var ageResults = RangeValidator
                .Validate(new RangeRequirements<int>()
                {
                    MinValue = 18,
                    MaxValue = 35,
                }
                          , value.Age
                          , "Age"
                          , value);

            return firstNameResults
                .Union(lastNameResults)
                .Union(ageResults)
                ;
        }
    }
}
