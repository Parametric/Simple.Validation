using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Personnel.Sample.DataModels
{
    public class Manager : Employee
    {
        public Manager()
        {
            this.Reports = new Collection<Employee>();
        }

        public ICollection<Employee> Reports { get; set; } 
    }
}
