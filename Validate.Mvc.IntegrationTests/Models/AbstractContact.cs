namespace Validate.Mvc.IntegrationTests.Models
{
    public class AbstractContact
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
}