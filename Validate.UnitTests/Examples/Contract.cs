using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validate.UnitTests.Examples
{
    public class Contact
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string PersonalEmail { get; set; }
        public string BusinessEmail { get; set; }

        public Address BusinessAddress { get; set; }
        public Address OtherAddress { get; set; }

        public Company Organization { get; set; }
        public Job CurrentJob { get; set; }
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

    public class Company
    {
        public string Name { get; set; }
        public Address OfficeAddress { get; set; }
    }

    public class Job
    {
        public string Title { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
    }
}
