﻿namespace ResumeSystem.ResultModels
{
    /// <summary>
    /// 当点击单一简历时，返回的类
    /// </summary>
    public class SingleResumeModelClass
    {
        public int ResuemId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string JobPosition { get; set; } // 求职意向岗位
        public string SelfEvaluation { get; set; } // 自我评价
        public string HighestEducation { get; set; } // 最高学历
        public string GraduatedFrom { get; set; } // 毕业院校
        public string SkillCertificate { get; set; } // 技能证书
        public string Awards { get; set; } // 获奖荣誉
        public string WorkExperience { get; set; } // 各段工作经历
        public int TotalWorkYears { get; set; } // 工作总时间
        public int MatchingScore { get; set; }
        public string TalentTraits { get; set; }
    }
}
