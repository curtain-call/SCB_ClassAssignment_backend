namespace ResumeSystem.Class
{
    public class Applicant
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string JobIntention { get; set; } // 求职意向岗位
        public string SelfEvaluation { get; set; } // 自我评价
        public string HighestEducation { get; set; } // 最高学历
        public string GraduatedFrom { get; set; } // 毕业院校
        public string SkillCertificate { get; set; } // 技能证书
        public string Awards { get; set; } // 获奖荣誉
        public string WorkExperience { get; set; } // 各段工作经历
        public int TotalWorkYears { get; set; } // 工作总时间
        public string WorkStability { get; set; } // 工作稳定性的程度
        public string StabilityReason { get; set; } // 工作稳定性判断的原因

        // 导航属性
        public Resume Resume { get; set; }
        public ApplicantProfile ApplicantProfile { get; set; }
    }
}
