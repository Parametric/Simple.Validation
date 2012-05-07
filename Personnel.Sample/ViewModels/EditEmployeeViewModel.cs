using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Personnel.Sample.DataModels;

namespace Personnel.Sample.ViewModels
{
    public class EditEmployeeViewModel
    {
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [Range(18, 65)]
        public int Age { get; set; }

        public string BirthDate { get; set; }

        public decimal Salary { get; set; }

        public string ReportsTo { get; set; }

        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string PostalCode { get; set; }
        public string StateOrProvince { get; set; }
        public string Country { get; set; }

        public IList<ContactInfo> ContactInfo { get; set; }

        public string Title { get; set; }

        public bool IsSalaried { get; set; }

        public bool IsHourly { get; set; }
    }
}
