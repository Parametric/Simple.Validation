using System.Collections.Generic;
using System.Linq;
using Simple.Validation;
using Simple.Validation.Validators;

namespace Personnel.Sample.Validators
{
    public class EmailAddressValidator : IValidator<EmailAddress>
    {
        public bool AppliesTo(string rulesSet)
        {
            return true;
        }

        public IEnumerable<ValidationResult> Validate(EmailAddress value)
        {
            var internalValidator = new SaveContactInfoValidator();
            var results = internalValidator.Validate(value).ToList();

            const string emailExpression = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                           @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                           @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            var emailResults = Properties<EmailAddress>
                .For(e => e.Text)
                .Required()
                .Matches(emailExpression)
                .Validate(value);

            results.AddRange(emailResults);
            return results;
        }

    }
}
