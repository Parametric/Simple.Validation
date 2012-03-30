using System.Collections.Generic;
using Simple.Validation;
using Simple.Validation.Validators;

namespace Personnel.Sample.Validators
{
    public class CreateNewEmployeeValidator : CompositeValidator<Employee>
    {
        public override bool AppliesTo(string rulesSet)
        {
            return rulesSet == EmployeeOperations.CreateNewEmployee;
        }

        protected override IEnumerable<IValidator<Employee>> GetInternalValidators()
        {
            yield return Properties<Employee>
                .For(e => e.FirstName)
                .Length(3, 50)
                .Required()
                .IgnoreWhiteSpace();

            yield return Properties<Employee>
                .For(e => e.LastName)
                .Length(3, 50)
                .Required()
                .IgnoreWhiteSpace()
                ;

            yield return Properties<Employee>
                .For(e => e.Age)
                .MinValue(18)
                .MaxValue(35)
                ;

            yield return Properties<Employee>
                .For(e => e.Address)
                .Required()
                .Cascade("Save")
                ;

            yield return Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Size(1)
                .Unique<ContactInfo>(c => c.Type)
                .Cascade("Save");

        }
    }
}