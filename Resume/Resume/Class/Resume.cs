namespace ResumeSystem.Class
{
    public class Resume
    {
        public int ID { get; set; }
        public int ApplicantID { get; set; }
        public int JobPositionID { get; set; }
        public string Content { get; set; }

        // 导航属性
        public Applicant Applicant { get; set; }
        public JobPosition JobPosition { get; set; }
    }
}
