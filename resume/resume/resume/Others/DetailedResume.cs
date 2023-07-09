using resume.Models;
namespace resume.Others
{
    public class DetailedResume
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string JobIntention { get; set; }
        public string SelfEvaluation { get; set; }
        public string HighestEducation { get; set; }
        public string WorkStabilityReason { get; set; }

        public string WorkStability { get; set; }
        public int MatchingScore { get; set; }
        public string MatchingReason { get; set; }
        public string TalentTraits { get; set; }
        public List<AwardInfo> Awards { get; set; }
        public List<WorkExperience> WorkExperience { get; set; }
        public List<SkillCertificate> SkillCertificate { get; set; }
        public List<EducationBackground> EducationBackgrounds { get; set; }

    }
}
