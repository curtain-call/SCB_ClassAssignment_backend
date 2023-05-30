namespace ResumeSystem.Class
{
    public class ApplicantProfile
    {
        public int ID { get; set; }
        public int ApplicantID { get; set; }
        public string TalentTraits { get; set; }
        public string MatchingReason { get; set; }
        public int OverallScore { get; set; }

        // 导航属性
        public Applicant Applicant { get; set; }
    }
}
