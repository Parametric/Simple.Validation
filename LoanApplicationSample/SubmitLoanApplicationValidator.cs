using System.Collections.Generic;
using System.Linq;
using Simple.Validation;
using Simple.Validation.Validators;

namespace LoanApplicationSample
{
    public class SubmitLoanApplicationValidator : IValidator<LoanApplication>
    {
        const int MinLoanAmount = 50000;
        const int MaxLoanAmount = 1500000;
        const int MinCreditScore = 500;
        const int MaxCreditScore = 830;

        public bool AppliesTo(string rulesSet)
        {
            return rulesSet == "Submit";
        }

        public IEnumerable<ValidationResult> Validate(LoanApplication value)
        {
            var loanAmountResults = Properties<LoanApplication>
                .For(e => e.LoanAmount)
                .GreaterThanOrEqualTo(MinLoanAmount)
                .LessThanOrEqualTo(MaxLoanAmount)
                .Message("Loan amount must be between {0:C} and {1:C}", MinLoanAmount, MaxLoanAmount)
                .Validate(value)
                ;

            var creditScoreResults = Properties<LoanApplication>
                .For(e => e.CreditScore)
                .GreaterThanOrEqualTo(MinCreditScore)
                .LessThanOrEqualTo(MaxCreditScore)
                .Message("Credit score must be between {0} and {1}", MinCreditScore, MaxCreditScore)
                .Validate(value)
                ;

            var results = loanAmountResults.Concat(creditScoreResults);
            return results;
        }
    }
}