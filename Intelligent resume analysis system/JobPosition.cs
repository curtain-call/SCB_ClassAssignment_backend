using System;

namespace IntelligentResumeParsingSystem.Models
{
    public class JobRequirements
    {
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Skills { get; set; }
        // add more properties as needed
    }

    public class JobPosition
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public JobRequirements Requirements { get; set; }
        public string SalaryRange { get; set; }
        public int CompanyId { get; set; }
    }
}