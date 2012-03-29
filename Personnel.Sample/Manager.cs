using System.Collections.Generic;

namespace Personnel.Sample
{
    public class Manager : Employee
    {
        public ICollection<Employee> Reports { get; set; } 
    }
}
