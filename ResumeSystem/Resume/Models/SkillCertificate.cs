namespace ResumeSystem.Models
{
    public class SkillCertificate
    {
        public int Id { get; set; }
        public int ApplicantID { get; set; }
        public string SkillName { get; set; }
        public Applicant Applicant { get; set; }
    }
}
