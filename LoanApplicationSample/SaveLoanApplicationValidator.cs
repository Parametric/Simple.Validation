using System.Collections.Generic;
using System.Linq;
using Simple.Validation;

namespace LoanApplicationSample
{
    public class SaveLoanApplicationValidator : IValidator<LoanApplication>
    {
        public bool AppliesTo(string rulesSet)
        {
            return rulesSet == "Save";
        }

        public IEnumerable<ValidationResult> Validate(LoanApplication value)
        {
            var nameResults = Properties<LoanApplication>
                .For(e => e.Name)
                .Required()
                .Length(2, 100)
                .Message("Name is required.")
                .IgnoreWhiteSpace()
                .Validate(value)
                ;

            var reasonResults = Properties<LoanApplication>
                .For(e => e.Reason)
                .Required()
                .Length(10, 500)
                .IgnoreWhiteSpace()
                .Message("Reason is required.")
                .Validate(value)
                ;


            var accumulatedResults = nameResults
                .Concat(reasonResults)
            ;

            return accumulatedResults;
        }
    }
}
