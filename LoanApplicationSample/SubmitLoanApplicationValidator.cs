using System.Collections.Generic;
using Simple.Validation;

namespace LoanApplicationSample
{
    public class SubmitLoanApplicationValidator : IValidator<LoanApplication>
    {
        public bool AppliesTo(string rulesSet)
        {
            return rulesSet == "Submit";
        }

        public IEnumerable<ValidationResult> Validate(LoanApplication value)
        {
            var loanAmountResults = RangeValidator
                .For<LoanApplication, long>(e => e.LoanAmount)
                .GreaterThanOrEqualTo(50000)
                .LessThanOrEqualTo(1500000)
                ;

            var creditScoreResults = RangeValidator
                .For<LoanApplication>
                .GreaterThanOrEqualTo(500)
                .LessThanOrEqualTo(830)
                ;

        }
    }
}