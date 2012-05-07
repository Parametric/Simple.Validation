using System.Collections.Generic;

namespace Personnel.Sample.DataModels
{
    public class Manager : Employee
    {
        public ICollection<Employee> Reports { get; set; } 
    }
}
