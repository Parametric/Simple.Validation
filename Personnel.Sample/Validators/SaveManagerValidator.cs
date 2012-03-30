using System.Collections.Generic;
using Simple.Validation;
using Simple.Validation.Validators;

namespace Personnel.Sample.Validators
{
    public class SaveManagerValidator : CompositeValidator<Manager>
    {
        public override bool AppliesTo(string rulesSet)
        {
            return rulesSet == RulesSets.Crud.Save;
        }

        protected override IEnumerable<IValidator<Manager>> GetInternalValidators()
        {
            yield return Properties<Manager>
                .For(m => m.Reports)
                .Required()
                .Count(minCount:1, maxCount:null)
                .Severity(ValidationResultSeverity.Warning)
                .Message("Manager should have at least 1 report!")
                .Unique<Employee>(e => e.EmployeeId)
                .Cascade(RulesSets.Crud.Save)
                ;

            yield return new AncestorTypeValidator<Manager, Employee>();
        }
    }

}
