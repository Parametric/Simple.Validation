using System.ComponentModel.DataAnnotations;

namespace Simple.Validation.Tests.TestDomain
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
    }
}