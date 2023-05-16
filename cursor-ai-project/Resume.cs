using System;

namespace IntelligentResumeParsingSystem.Models
{
    public class Resume
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string HighestEducation { get; set; }
        public string University { get; set; }
        public int WorkExperience { get; set; }
    }
}
