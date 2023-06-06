using System.Collections.Generic;

namespace ResumeSystem.Models
{
    public class Company
    {
        public int ID { get; set; }
        public string Account { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // 新增的JobPositions属性
        public ICollection<JobPosition> JobPositions { get; set; }

        // 新增的Resumes属性
        public ICollection<Resume> Resumes { get; set; }
    }
}
