using Microsoft.EntityFrameworkCore;
using resume.Models;
using resume.ResultModels;

namespace resume.Service
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
                                .Include(r => r.Applicant.EducationBackgrounds)
                                .FirstOrDefault(r => r.ID == resumeId);

            if (resume == null)
            {
                return null;
            }

            var detailedResume = new DetailedResume
            {
                Id = resumeId,
                Age = resume.Applicant.Age,
                Name = resume.Applicant.Name,
                Email = resume.Applicant.Email,
                PhoneNumber = resume.Applicant.PhoneNumber,
                JobIntention = resume.JobPosition.Title,
                Gender = resume.Applicant.Gender,
                SelfEvaluation = resume.Applicant.SelfEvaluation,
                HighestEducation = resume.Applicant.HighestEducation,
                //TalentTraits = resume.Applicant.ApplicantProfile.TalentTraits,
                Awards = resume.Applicant.Awards?.Select(a => new AwardInfo { AwardName = a.AwardName }).ToList() ?? new List<AwardInfo>(), // assign Awards
                WorkExperience = resume.Applicant.WorkExperiences?.ToList() ?? new List<WorkExperiences>(),  // assign WorkExperiences
                SkillCertificate = resume.Applicant.SkillCertificates?.ToList() ?? new List<SkillCertificate>(),  // assign SkillCertificates
                WorkStability = resume.Applicant.ApplicantProfile.WorkStability,
                WorkStabilityReason = resume.Applicant.ApplicantProfile.StabilityReason,
                MatchingScore = resume.Applicant.ApplicantProfile.MatchingScore,
                MatchingReason = resume.Applicant.ApplicantProfile.MatchingReason,
                EducationBackgrounds = resume.Applicant.EducationBackgrounds?.ToList() ?? new List<EducationBackground>()
            };

            return detailedResume;
        }

        /*public void UpdateFilePath(string filePath, int resumeID)
        {
            var existingResume = _dbContext.Resumes
            .FirstOrDefault(r => r.ID == resumeID);

            if (existingResume != null)
            {
                existingResume.FilePath = filePath;
                _dbContext.SaveChanges();
            }
        }*/

        public int AddResumePath(string filePath, Applicant applicantResult, int companyID)
        {
            var resume = new Resume
            {
                FilePath = filePath,
                ApplicantID = applicantResult.ID,
                CompanyID = companyID,
                JobPositionID = 1,
                Applicant = applicantResult,
            };
            _dbContext.Resumes.Add(resume);
            _dbContext.SaveChanges();
            Console.WriteLine("内" + resume.ID);
            return resume.ID;
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
