namespace resume.Models
{
    public class ResumeDTO
    {
        public int ResumeId { get; set; }
        public string? ApplicantName { get; set; }
        public string? JobPosition { get; set; }
        public int MatchingScore { get; set; }
    }
}
