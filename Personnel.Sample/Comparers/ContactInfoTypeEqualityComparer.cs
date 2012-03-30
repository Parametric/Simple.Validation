using System.Collections.Generic;

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
