using System;
using System.ComponentModel.DataAnnotations;
using Simple.Validation.Tests.TestDomain;

namespace Personnel.Sample
{
    public class Employee : IEmployee
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [Range(18, 35)]
        public int Age { get; set; }

        public double? Salary { get; set; }

        public Manager ReportsTo { get; set; }

        public Address Address { get; set; }
    }
}