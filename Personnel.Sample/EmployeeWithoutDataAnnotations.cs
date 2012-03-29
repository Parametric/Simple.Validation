namespace Simple.Validation.Tests.TestDomain
{
    public class EmployeeWithoutDataAnnotations : IEmployee
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }       
    }
}