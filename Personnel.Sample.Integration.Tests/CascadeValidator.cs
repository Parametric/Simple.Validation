using System.Collections.Generic;
using Personnel.Sample.DataModels;
using Simple.Validation;
using Simple.Validation.Validators;

namespace Personnel.Sample.Integration.Tests
{
    public class CascadeValidator : CompositeValidator<Manager>
    {
        public override bool AppliesTo(string rulesSet)
        {
            return true;
        }

        protected override IEnumerable<IValidator<Manager>> GetInternalValidators()
        {
            yield return Properties<Manager>
                .For(row => row.Reports)
                .Cascade()
                ;
        }
    }
}