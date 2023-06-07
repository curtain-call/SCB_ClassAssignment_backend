namespace ResumeSystem.Models
{
    public class Resume
    {
        public int ID { get; set; }
        public int ApplicantID { get; set; }
        public int JobPositionID { get; set; }
        public int CompanyID { get; set; } // 新增的CompanyID属性
        public string? Content { get; set; }
        public string? FilePath { get; set; }

        // 导航属性
        public Applicant Applicant { get; set; }
        public JobPosition JobPosition { get; set; }
        public Company Company { get; set; }

    }
}
