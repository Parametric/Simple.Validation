using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Validation;
using Simple.Validation.Validators;

namespace Personnel.Sample.Validators
{
    public class SaveContactInfoValidator : CompositeValidator<ContactInfo>
    {
        private readonly string[] _validValues = new[]
                                          {
                                              "Email", "Home Phone", "Work Phone", "Mobile Phone", "Fax"
                                          };

        public override bool AppliesTo(string rulesSet)
        {
            return rulesSet == "Save";
        }

        protected override IEnumerable<IValidator<ContactInfo>> GetInternalValidators()
        {
            yield return Properties<ContactInfo>
                .For(c => c.Type)
                .Required()
                .IgnoreWhiteSpace()
                .Length(1, 20)
                ;

            yield return Properties<ContactInfo>
                .For(c => c.Type)
                .Required()
                .IgnoreWhiteSpace()
                .IsTrue(value => _validValues.Contains(value))
                .Severity(ValidationResultSeverity.Warning)
                ;

            yield return Properties<ContactInfo>
                .For(c => c.Text)
                .Required()
                .IgnoreWhiteSpace()
                .Length(1, 20);

            yield return Properties<ContactInfo>
                .For(c => c.Notes)
                .NotRequired()
                .Length(0, 200)
                .IgnoreWhiteSpace()
                ;

        }
    }
}
