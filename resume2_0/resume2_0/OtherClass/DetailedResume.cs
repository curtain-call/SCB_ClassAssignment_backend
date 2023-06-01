using System.Globalization;

namespace resume2_0.OtherClass
{
    public class DetailedResume
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string JobIntention { get; set; }
        public string SelfEvaluation { get; set; }
        public List<string> Awards { get; set; }
        public List<WorkExperience> workExperiences { get; set; }
        public string WorkStabilityReason { get; set; }
        public string WorkStability { get; set; }
        public int MatchingScore { get; set;}
        public string MatchingReason { get; set; }
    }
}
