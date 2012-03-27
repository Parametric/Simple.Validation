using System.ComponentModel.DataAnnotations;

namespace Simple.Validation.Tests.TestDomain
{
    public interface IEmployee
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        string LastName { get; set; }

        [Range(18, 35)]
        int Age { get; set; }
    }
}