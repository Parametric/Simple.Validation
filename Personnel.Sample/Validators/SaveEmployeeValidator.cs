using System.Collections.Generic;
using Simple.Validation;
using Simple.Validation.Validators;

namespace Personnel.Sample.Validators
{
    public class SaveEmployeeValidator : CompositeValidator<Employee>
    {
        public override bool AppliesTo(string rulesSet)
        {
            return rulesSet == RulesSets.Crud.Save;
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
                .MaxValue(65)
                ;

            yield return Properties<Employee>
                .For(e => e.Address)
                .Required()
                .Cascade("Save")
                ;

            yield return Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Count(1)
                .Unique<ContactInfo>(c => c.Type)
                .Cascade("Save");

        }
    }
}