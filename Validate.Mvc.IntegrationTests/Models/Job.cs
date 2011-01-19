using System;

namespace Validate.Mvc.IntegrationTests.Models
{
    public class Job
    {
        public string Title { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
    }
}