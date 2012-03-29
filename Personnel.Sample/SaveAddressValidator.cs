﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Validation;

namespace Personnel.Sample
{
    public class SaveAddressValidator : IValidator<Address>
    {
        public bool AppliesTo(string rulesSet)
        {
            return rulesSet == "Save";
        }

        public IEnumerable<ValidationResult> Validate(Address value)
        {
            var propertyValidators = GetPropertyValidators();
            var results = propertyValidators.SelectMany(v => v.Validate(value));
            return results;
        }

        private IEnumerable<IValidator<Address>> GetPropertyValidators()
        {
            yield return Properties<Address>.
                For(a => a.Line1)
                .Length(0, 50)
                .Required()
                .Message("Address Line 1 is required.");

            yield return Properties<Address>
                .For(a => a.Line2)
                .Length(0, 50)
                .NotRequired()
                .Message("Line 2 must be between 0 and 50 characters in length.");

            yield return Properties<Address>
                .For(a => a.Line3)
                .Length(0, 50)
                .NotRequired()
                .Message("Line 3 must be between 0 and 50 characters in length.");

            yield return Properties<Address>
                .For(a => a.PostalCode)
                .Length(0, 20)
                .NotRequired()
                .Message("Postal Code must be between 0 and 20 characters in length.");

            yield return Properties<Address>
                .For(a => a.Country)
                .Length(0, 3)
                .NotRequired()
                .Message("Country must be between 0 and 3 characters in length.");

            yield return Properties<Address>
                .For(a => a.StateOrProvince)
                .Length(0, 50)
                .NotRequired()
                .Message("StateOrProvince must be between 0 and 50 characters in length.");

        } 
    }
}
