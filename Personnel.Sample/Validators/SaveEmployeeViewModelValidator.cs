using System;
using System.Collections.Generic;
using Personnel.Sample.DataModels;
using Personnel.Sample.ViewModels;
using Simple.Validation;
using Simple.Validation.Validators;

namespace Personnel.Sample.Validators
{
    public class SaveEmployeeViewModelValidator : CompositeValidator<EditEmployeeViewModel>
    {
        private const int MinimumHireAge = 18;
        private const int MaximumHireAge = 65;
        private const int MinimumNameLength = 3;
        private const int MaximumNameLength = 50;

        public override bool AppliesTo(string rulesSet)
        {
            return rulesSet == RulesSets.Crud.Save;
        }

        protected override IEnumerable<IValidator<EditEmployeeViewModel>> GetInternalValidators()
        {
            yield return For(e => e.FirstName)
                .Length(MinimumNameLength, MaximumNameLength)
                .Required()
                .IgnoreWhiteSpace();

            yield return For(e => e.LastName)
                .Length(MinimumNameLength, MaximumNameLength)
                .Required()
                .IgnoreWhiteSpace()
                .Message("Last name is a required field and must be between {0} an {1} characters in length."
                         , MinimumNameLength, MaximumNameLength)
                ;

            yield return For(e => e.Age)
                .MinValue(MinimumHireAge)
                .MaxValue(MaximumHireAge)
                .Message("Employees must be between the ages of {0} and {1}"
                         , MinimumHireAge, MaximumHireAge)
                ;

            yield return For(e => e.Line1)
                .Required()
                .Message("Address is required.")
                ;

            yield return For(e => e.ContactInfo)
                .Required()
                .Count(1)
                .Unique<ContactInfo>(c => c.Type)
                .Cascade("Save")
                ;

            yield return For(e => e.ReportsTo)
                .Required()
                .If(e => e.Title != "Chief Executive Officer")
                ;

            yield return For(e => e.IsSalaried)
                .Assert((e, p) => e.IsSalaried == !e.IsHourly)
                .Message("An employee cannot be both hourly and salaried.")
                ;

            yield return For(e => e.BirthDate)
                .Assert((e, p) =>
                            {
                                DateTime output;
                                return DateTime.TryParse(e.BirthDate, out output);
                            })
                .Message("BirthtDate is not a valid DateTime.");
        }
    }
}