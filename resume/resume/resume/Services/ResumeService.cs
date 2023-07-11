using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using resume.Models;
using resume.Others;
using resume.ResultModels;
using resume.WebSentModel;

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
                WorkExperience = resume.Applicant.WorkExperiences?.ToList() ?? new List<WorkExperience>(),  // assign WorkExperiences
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

        public GraphForJonResumeCountModelClass ForJonResumeCount(int userId)
        {
            var company = _dbContext.Users
                                    .Include(u => u.Company)
                                    .ThenInclude(c => c.JobPositions)
                                    .ThenInclude(jp => jp.Resumes)
                                    .FirstOrDefault(u => u.ID == userId)
                                    ?.Company;

            if (company == null)
            {
                // Handle this error according to your business requirements
                // For example, you might want to throw an exception, or return a response indicating that the user or company was not found
            }

            var totalResumes = company.Resumes.Count;

            var jobResumeCounts = company.JobPositions.Select(jp => new JobResumeCount
            {
                JobName = jp.Title,
                ResumeCount = jp.Resumes.Count
            }).ToList();

            var result = new GraphForJonResumeCountModelClass
            {
                TotalResumes = totalResumes,
                JobResumeCounts = jobResumeCounts
            };

            return result;
        }

        public EducationInfoForGraphClass ForGraphByEducation(int userId)
        {
            var company = _dbContext.Users
                            .Include(u => u.Company)
                            .ThenInclude(c => c.Resumes)
                            .ThenInclude(r => r.Applicant)
                            .FirstOrDefault(u => u.ID == userId)
                            ?.Company;

            if (company == null)
            {
                // Handle this error according to your business requirements
            }

            var highestEducationCounts = new HighestEducation();
            var graduationSchoolsLevelCounts = new GraduationSchoolsLevel();

            foreach (var resume in company.Resumes)
            {
                switch (resume.Applicant.HighestEducation)
                {
                    case "无":
                    case "小学":
                    case "初中":
                    case "高中":
                    case "中专":
                        highestEducationCounts.HighSchoolOrLess++;
                        break;
                    case "大专":
                        highestEducationCounts.JuniorCollege++;
                        break;
                    case "本科":
                        highestEducationCounts.Bachelor++;
                        break;
                    case "硕士":
                        highestEducationCounts.Master++;
                        break;
                    case "博士":
                        highestEducationCounts.Doctor++;
                        break;
                }

                switch (resume.Applicant.GraduatedFromLevel)
                {
                    case "985":
                        graduationSchoolsLevelCounts._985++;
                        break;
                    case "211":
                        graduationSchoolsLevelCounts._211++;
                        break;
                    case "普通一本":
                        graduationSchoolsLevelCounts.OrdinaryFirstClass++;
                        break;
                    case "一本以下":
                        graduationSchoolsLevelCounts.SecondClassOrBelow++;
                        break;
                }
            }

            var result = new EducationInfoForGraphClass
            {
                HighestEducation = highestEducationCounts,
                GraduationSchoolsLevel = graduationSchoolsLevelCounts
            };

            return result;      
        }

        public AgeInfoForGraphClass AgeInfoForGraphClass(int userId)
        {
            var company = _dbContext.Users
                                    .Include(u => u.Company)
                                    .ThenInclude(c => c.Resumes)
                                    .ThenInclude(r => r.Applicant)
                                    .FirstOrDefault(u => u.ID == userId)
                                    ?.Company;

            if (company == null)
            {
                // Handle this error according to your business requirements
            }

            var ageGroupCounts = new AgeGroups();

            foreach (var resume in company.Resumes)
            {
                var age = resume.Applicant.Age;
                if (age >= 18 && age < 22)
                {
                    ageGroupCounts.Age18_22++;
                }
                else if (age >= 22 && age < 25)
                {
                    ageGroupCounts.Age22_25++;
                }
                else if (age >= 25 && age < 30)
                {
                    ageGroupCounts.Age25_30++;
                }
                else if (age >= 30 && age < 35)
                {
                    ageGroupCounts.Age30_35++;
                }
                else if (age >= 35)
                {
                    ageGroupCounts.Age35AndAbove++;
                }
            }

            var result = new AgeInfoForGraphClass
            {
                AgeGroups = ageGroupCounts
            };

            return result;
        }

        public WorkYearInfoForGraphClass WorkYearInfoForGraph(int userId)
        {
            var company = _dbContext.Users
                                    .Include(u => u.Company)
                                    .ThenInclude(c => c.Resumes)
                                    .ThenInclude(r => r.Applicant)
                                    .FirstOrDefault(u => u.ID == userId)
                                    ?.Company;

            if (company == null)
            {
                // Handle this error according to your business requirements
            }

            var workYearCounts = new WorkYears();

            foreach (var resume in company.Resumes)
            {
                var totalWorkYears = resume.Applicant.TotalWorkYears;
                if (totalWorkYears <= 0)
                {
                    workYearCounts.Year0++;
                }
                else if (totalWorkYears <= 14)
                {
                    // Use reflection to increment the appropriate property
                    var property = typeof(WorkYears).GetProperty($"Year{totalWorkYears}");
                    if (property != null)
                    {
                        property.SetValue(workYearCounts, (int)property.GetValue(workYearCounts) + 1);
                    }
                }
                else if (totalWorkYears >= 15)
                {
                    workYearCounts.Above15++;
                }
            }

            var result = new WorkYearInfoForGraphClass
            {
                WorkYears = workYearCounts
            };

            return result;
        }

        public WorkStabilityInfoForGraphClass WorkStabilityInfoForGraph(int userId)
        {
            var company = _dbContext.Users
                                    .Include(u => u.Company)
                                    .ThenInclude(c => c.Resumes)
                                    .ThenInclude(r => r.Applicant)
                                    .ThenInclude(r => r.ApplicantProfile)
                                    .FirstOrDefault(u => u.ID == userId)
                                    ?.Company;

            if (company == null)
            {
                // Handle this error according to your business requirements
            }

            var workStabilityCounts = new WorkStability();

            foreach (var resume in company.Resumes)
            {
                var workStability = resume.Applicant.ApplicantProfile.WorkStability;
                switch (workStability)
                {
                    case "Low":
                        workStabilityCounts.Low++;
                        break;
                    case "MediumLow":
                        workStabilityCounts.MediumLow++;
                        break;
                    case "Medium":
                        workStabilityCounts.Medium++;
                        break;
                    case "MediumHigh":
                        workStabilityCounts.MediumHigh++;
                        break;
                    case "High":
                        workStabilityCounts.High++;
                        break;
                    default:
                        // handle this error according to your business requirements
                        break;
                }
            }

            var result = new WorkStabilityInfoForGraphClass
            {
                WorkStability = workStabilityCounts
            };

            return result;
        }

        public JobMatchScoresInfoForGraphClass JobMatchScoresInfoForGraph(int userId)
        {
            var company = _dbContext.Users
                                    .Include(u => u.Company)
                                    .ThenInclude(c => c.Resumes)
                                    .ThenInclude(r => r.Applicant)
                                    .ThenInclude(a => a.ApplicantProfile)
                                    .FirstOrDefault(u => u.ID == userId)
                                    ?.Company;

            if (company == null)
            {
                // Handle this error according to your business requirements
            }

            var jobMatchScoresCounts = new JobMatchScores();

            foreach (var resume in company.Resumes)
            {
                var matchingScore = resume.Applicant.ApplicantProfile.MatchingScore;

                if (matchingScore < 60)
                    jobMatchScoresCounts.Below60++;
                else if (matchingScore >= 60 && matchingScore < 70)
                    jobMatchScoresCounts.Range60_70++;
                else if (matchingScore >= 70 && matchingScore < 80)
                    jobMatchScoresCounts.Range70_80++;
                else if (matchingScore >= 80 && matchingScore < 90)
                    jobMatchScoresCounts.Range80_90++;
                else if (matchingScore >= 90 && matchingScore <= 100)
                    jobMatchScoresCounts.Range90_100++;
            }

            var result = new JobMatchScoresInfoForGraphClass
            {
                JobMatchScores = jobMatchScoresCounts
            };

            return result;
        }

    }
}
