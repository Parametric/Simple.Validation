using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Personnel.Sample
{
    public class Employee : IEmployee
    {
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [Range(18, 65)]
        public int Age { get; set; }

        public double? Salary { get; set; }

        public Manager ReportsTo { get; set; }

        public Address Address { get; set; }

        public IList<ContactInfo> ContactInfo { get; set; }

    }
}