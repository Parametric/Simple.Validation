using System.Collections.Generic;
using System.Linq;
using Simple.Validation;
using Simple.Validation.Validators;

namespace Personnel.Sample.Validators
{
    public class SaveContactInfoValidator : CompositeValidator<ContactInfo>
    {
        const string EmailExpression = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                               @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                               @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        private const string Email = "Email";
        private const string HomePhone = "Home Phone";
        private const string WorkPhone = "Work Phone";
        private const string MobilePhone = "Mobile Phone";
        private const string Fax = "Fax";
        private const string Custom = "Custom";

        private readonly string[] _validValues = new[]
                                          {
                                              Email, HomePhone, WorkPhone, MobilePhone, Fax, Custom
                                          };

        public override bool AppliesTo(string rulesSet)
        {
            return rulesSet == RulesSets.Crud.Save;
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
                .Length(1, 20)
                ;

            yield return Properties<ContactInfo>
                .For(c => c.Notes)
                .NotRequired()
                .Length(0, 200)
                .IgnoreWhiteSpace()
                ;

            yield return Properties<ContactInfo>
                .For(c => c.Text)
                .Matches(EmailExpression)
                .If(c => c.Type == Email)
                ;

        }
    }
}
