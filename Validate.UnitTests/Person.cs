using System.Collections.Generic;

namespace Validate.UnitTests
{
    internal class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int Goals { get; set; }
        public int Fouls { get; set; }
        public bool IsTrue { get; set; }
        public Address HomeAddress { get; set; }
        public IEnumerable<string> EmailAddresses { get; set; }

        #region Equality Members
        public bool Equals(Person other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Person)) return false;
            return Equals((Person) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
        #endregion

        public override string ToString()
        {
            return string.Format("Name: {0}", Name);
        }
    }

    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string City { get; set; }
        public string StateOrCounty { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
    }
}