using System.Collections.Generic;
using Simple.Validation.Tests.TestDomain;

namespace Personnel.Sample
{
    public class Manager
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public ICollection<Employee> Reports { get; set; } 
    }
}
