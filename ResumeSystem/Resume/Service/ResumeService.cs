using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ResumeSystem.Models;
using ResumeSystem.ResultModels;
using ResumeSystem.WebSentModel;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Crypto;

namespace ResumeSystem.Service
{
    public class ResumeService
    {
        private readonly MyDbContext _dbContext;

        public ResumeService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DetailedResume GetResumeById(int resumeId)
        {
            var resume = _dbContext.Resumes.Include(r => r.Applicant)
                                .ThenInclude(a => a.ApplicantProfile)
                                .Include(r => r.JobPosition)
                                .Include(r => r.Applicant.WorkExperiences)  // include WorkExperiences
                                .Include(r => r.Applicant.Awards) // include Awards
                                .Include(r => r.Applicant.SkillCertificates) // include SkillCertificates
                                .FirstOrDefault(r => r.ID == resumeId);

            if (resume == null)
            {
                return null;
            }

            var detailedResume = new DetailedResume
            {
                Age = resume.Applicant.Age,
                Name = resume.Applicant.Name,
                Email = resume.Applicant.Email,
                PhoneNumber = resume.Applicant.PhoneNumber,
                JobIntention = resume.JobPosition.Title,
                Gender = resume.Applicant.Gender,
                SelfEvaluation = resume.Applicant.SelfEvaluation,
                HighestEducation = resume.Applicant.HighestEducation,
                TalentTraits = resume.Applicant.ApplicantProfile.TalentTraits,
                Awards = resume.Applicant.Awards.ToList(),  // assign Awards
                WorkExperience = resume.Applicant.WorkExperiences.ToList(),  // assign WorkExperiences
                SkillCertificate = resume.Applicant.SkillCertificates.ToList(),  // assign SkillCertificates
                WorkStability = resume.ApplicantProfile.WorkStability,
                WorkStabilityReason = resume.ApplicantProfile.StabilityReason,
                MatchingScore = resume.ApplicantProfile.MatchingScore,
                MatchingReason = resume.ApplicantProfile.MatchingReason,
            };

            return detailedResume;
        }

        public void UpdateFilePath(string filePath, int resumeID)
        {
            var existingResume = _dbContext.Resumes
            .FirstOrDefault(r => r.ID == resumeID);

            if (existingResume != null)
            {
                existingResume.FilePath = filePath;
                _dbContext.SaveChanges();
            }
        }

        public string GetFilePathById(int resumeID)
        {
            var existingResume = _dbContext.Resumes
            .FirstOrDefault(r => r.ID == resumeID);

            if (existingResume != null)
            {
                return existingResume.FilePath;
            }
            return "";
        }
    }
}
