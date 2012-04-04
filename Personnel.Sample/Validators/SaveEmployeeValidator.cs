using System.Collections.Generic;
using Simple.Validation;
using Simple.Validation.Validators;

namespace Personnel.Sample.Validators
{
    public class SaveEmployeeValidator : CompositeValidator<Employee>
    {
        private const int MinimumHireAge = 18;
        private const int MaximumHireAge = 65;
        private const int MinimumNameLength = 3;
        private const int MaximumNameLength = 50;

        public override bool AppliesTo(string rulesSet)
        {
            return rulesSet == RulesSets.Crud.Save;
        }

        protected override IEnumerable<IValidator<Employee>> GetInternalValidators()
        {
            yield return Properties<Employee>
                .For(e => e.FirstName)
                .Length(MinimumNameLength, MaximumNameLength)
                .Required()
                .IgnoreWhiteSpace();

            yield return Properties<Employee>
                .For(e => e.LastName)
                .Length(MinimumNameLength, MaximumNameLength)
                .Required()
                .IgnoreWhiteSpace()
                .Message("Last name is a required field and must be between {0} an {1} characters in length."
                    , MinimumNameLength, MaximumNameLength)
                ;

            yield return Properties<Employee>
                .For(e => e.Age)
                .MinValue(MinimumHireAge)
                .MaxValue(MaximumHireAge)
                .Message("Employees must be between the ages of {0} and {1}"
                    , MinimumHireAge, MaximumHireAge)
                ;

            yield return Properties<Employee>
                .For(e => e.Address)
                .Required()
                .Message("Address is required.")
                .Cascade("Save")
                ;

            yield return Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Count(1)
                .Unique<ContactInfo>(c => c.Type)
                .Cascade("Save")
                ;

            yield return Properties<Employee>
                .For(e => e.ReportsTo)
                .Required()
                .If(e => e.Title != "Chief Executive Officer")
                ;
        }
    }
}