namespace Validate.Mvc.IntegrationTests.Models
{
    [ValidateUsing(typeof(ContactValidator))]
    public class Contact_2 : AbstractContact
    {
    }
}