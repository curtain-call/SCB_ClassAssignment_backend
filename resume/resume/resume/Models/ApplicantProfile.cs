namespace resume.Models
{
    public class ApplicantProfile
    {
        public int ID { get; set; }
        public int ApplicantID { get; set; }
        //public string? TalentTraits { get; set; }
        public string? MatchingReason { get; set; } //给出此工作稳定性判断的原因
        public int MatchingScore { get; set; }  //人岗匹配程度分数
        public string? WorkStability { get; set; } // 工作稳定性的程度
        public string? StabilityReason { get; set; } // 工作稳定性判断的原因
        // ...
        public ICollection<JobMatch> JobMatches { get; set; }
        public PersonalCharacteristics PersonalCharacteristics { get; set; }
        public SkillsAndExperiences SkillsAndExperiences { get; set; }
        public AchievementsAndHighlights AchievementsAndHighlights { get; set; }
        // 导航属性
        public Applicant Applicant { get; set; }
    }
}
