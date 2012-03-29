using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Personnel.Sample.Comparers
{
    public class ContactInfoTypeEqualityComparer : IEqualityComparer<ContactInfo>
    {
        public bool Equals(ContactInfo x, ContactInfo y)
        {
            return x.Type == y.Type;
        }

        public int GetHashCode(ContactInfo obj)
        {
            return obj.GetHashCode();
        }
    }
}
