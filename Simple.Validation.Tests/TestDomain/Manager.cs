using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Validation.Tests.TestDomain
{
    public class Manager
    {
        public string Name { get; set; }
        public ICollection<Employee> Reports { get; set; } 
    }
}
