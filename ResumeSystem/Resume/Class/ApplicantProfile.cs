namespace ResumeSystem.Class
{
    public class ApplicantProfile
    {
        public int ID { get; set; }
        public int ApplicantID { get; set; }
        public string TalentTraits { get; set; }
        public string MatchingReason { get; set; }
        public int MatchingScore { get; set; }
        public string WorkStability { get; set; } // 工作稳定性的程度
        public string StabilityReason { get; set; } // 工作稳定性判断的原因
        // 导航属性
        public Applicant Applicant { get; set; }
    }
}
    