using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoanApplicationSample
{
    public class LoanApplication
    {
        public string Name { get; set; }
        public string Reason { get; set; }

        public long LoanAmount { get; set; }

        public int CreditScore { get; set; }
    }
}
