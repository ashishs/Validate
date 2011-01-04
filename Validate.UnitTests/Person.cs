namespace Validate.UnitTests
{
    internal class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int Goals { get; set; }
        public int Fouls { get; set; }
        public bool IsTrue { get; set; }

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
}