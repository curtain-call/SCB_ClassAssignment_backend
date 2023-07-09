namespace resume.Models
{
    public class Company
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public ICollection<JobPosition> JobPositions { get; set; }
        public ICollection<Resume> Resumes { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
