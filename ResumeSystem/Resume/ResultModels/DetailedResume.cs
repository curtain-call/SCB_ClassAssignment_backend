﻿using ResumeSystem.Models;
using ResumeSystem.ResultModel;

namespace ResumeSystem.ResultModels
{
    public class DetailedResume
    {   
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public string? JobIntention { get; set; }
        public string? SelfEvaluation { get; set; }
        public string? HighestEducation { get; set; }
        public string? WorkStabilityReason { get; set; }
        public string? WorkStability { get; set; }
        public int MatchingScore { get; set;}
        public string? MatchingReason { get; set; }
        public string? TalentTraits { get; set; }

        public ICollection<Award> Awards { get; set; }
        public ICollection<WorkExperiences> WorkExperience { get; set; }
        public ICollection<SkillCertificate> SkillCertificate { get; set; }

        public DetailedResume()
        {
            Name = "No name provided";
            Email = "No email provided";
            PhoneNumber = "No phone number provided";
            Awards = new List<Award>();
            WorkExperience = new List<WorkExperiences>();
            SkillCertificate = new List<SkillCertificate>();
        }
    }
}
