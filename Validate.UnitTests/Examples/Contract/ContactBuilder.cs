using System;

namespace Validate.UnitTests.Examples.Contract
{
    public class ContactBuilder
    {
        private Contact _contact;

        public ContactBuilder()
        {
            _contact = new Contact();
        }

        public ContactBuilder WithFirstName()
        {
            _contact.FirstName = "FirstName";
            return this;
        }

        public ContactBuilder WithLastName()
        {
            _contact.LastName = "LastName";
            return this;
        }

        public ContactBuilder WithBusinessAddress()
        {
            _contact.BusinessAddress = new Address { AddressLine1 = "Business Line 1", AddressLine2 = "Business Line 2", City = "Business City", StateOrCounty = "Business State", Country = "Business Country", Zipcode = "Business Zipcode" };
            return this;
        }

        public ContactBuilder WithOtherAddress()
        {
            _contact.OtherAddress = new Address { AddressLine1 = "Other Line 1", AddressLine2 = "Other Line 2", City = "Other City", StateOrCounty = "Other State", Country = "Other Country", Zipcode = "Other Zipcode" };
            return this;
        }

        public ContactBuilder WithOrganization()
        {
            _contact.Organization = new Company
                                        {
                                            Name = "Company", 
                                            OfficeAddress = new Address
                                                                {
                                                                    AddressLine1 = "Company Line 1", AddressLine2 = "Company Line 2", City = "Company City", StateOrCounty = "Company State", Country = "Company Country", Zipcode = "Company Zipcode"
                                                                }
                                        };
            return this;
        }

        public ContactBuilder WithCurrentJob()
        {
            _contact.CurrentJob = new Job {Title = "JobTitle",From = new DateTime(2010, 1, 1), To = null};
            return this;
        }

        public ContactBuilder WithPersonalEmail()
        {
            _contact.PersonalEmail = "personalemail@validate.codeplex.com";
            return this;
        }

        public ContactBuilder WithBusinessEmail()
        {
            _contact.BusinessEmail = "businessemail@validate.codeplex.com";
            return this;
        }

        public Contact GetContact()
        {
            return _contact;
        }
    }
}