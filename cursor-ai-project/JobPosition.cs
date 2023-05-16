using System;

namespace IntelligentResumeParsingSystem.Models
{
    public class JobPosition
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string SalaryRange { get; set; }
        public int CompanyId { get; set; }
    }
}
