namespace resume.Models
{
    public class Applicant
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }  //个人邮箱
        public string? PhoneNumber { get; set; }  //手机号
        public int Age { get; set; }
        public string? Gender { get; set; }
        public string? JobIntention { get; set; } // 求职意向岗位
        public string? HighestEducation { get; set; } // 最高学历
        public string? Major { get; set; } //专业
        public string? GraduatedFrom { get; set; } // 毕业院校
        public string? GraduatedFromLevel { get; set; } //毕业院校等级
        public string? SelfEvaluation { get; set; } // 自我评价        
        public int TotalWorkYears { get; set; } // 工作总时间

        // 导航属性
        public Resume Resume { get; set; }
        public ApplicantProfile ApplicantProfile { get; set; }
        public ICollection<Award> Awards { get; set; }  //获奖荣誉
        public ICollection<WorkExperience> WorkExperiences { get; set; }// 各段工作经历
        public ICollection<SkillCertificate> SkillCertificates { get; set; } //技能证书
        public ICollection<EducationBackground> EducationBackgrounds { get; set; } //教育背景
    }
}
