namespace ResumeSystem.Class
{
    public class Applicant
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public int WorkExperience { get; set; }
    public string Stability { get; set; }
    public string GraduatedFrom { get; set; }
    public string EducationLevel { get; set; }
    public string Awards { get; set; }

    // 导航属性
    public ApplicantProfile Profile { get; set; }
    public Resume Resume { get; set; }
}
}
