﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Validation;

namespace LoanApplicationSample
{
    public class LoanApplicationDb
    {
        public void Save(LoanApplication loanApplication)
        {
            Validator.Enforce(loanApplication, "Save");
            // do save logic
        }

        public void Submit(LoanApplication loanApplication)
        {
            Validator.Enforce(loanApplication, "Save", "Submit");
            // do submit logic
        }
    }
}
