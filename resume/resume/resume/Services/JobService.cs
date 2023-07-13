using Microsoft.EntityFrameworkCore;
using resume.Models;
using resume.ResultModels;
using resume.WebSentModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace resume.Services
{
    public class JobService
    {
        private readonly MyDbContext _dbContext;

        public JobService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public JobIdResultClass UploadJob(JobInfoSentModel jobInfo)
        {
            var userId = jobInfo.UserId;/* 你需要提供一个方式来获取当前登录用户的Id */
                
            var company = _dbContext.Users
                                    .Include(u => u.Company)
                                    .FirstOrDefault(u => u.ID == userId)
                                    ?.Company;
            if (company == null)
            {
                // 这可能意味着没有找到与userId关联的Company
                // 在这种情况下，你可能需要返回一个错误信息，而不是继续执行后面的代码
                return new JobIdResultClass();
            }

            var newJob = new JobPosition
            {
                CompanyID = company.ID,
                Title = jobInfo.JobName,
                Description = jobInfo.JobDetails,
                //Options = string.Join(",", jobInfo.Options), // 这假设Options是以逗号分隔的字符串存储的
                CreatedDate = DateTime.Now,
                MinimumWorkYears = jobInfo.MinimumWorkYears,
                MinimumEducationLevel = jobInfo.MinimumEducationLevel
            };

            _dbContext.JobPositions.Add(newJob);
            _dbContext.SaveChanges();

            return new JobIdResultClass { Id = newJob.ID };
        }

        public JobMatchResultModelClass JobMatchResume(int userId, int jobId)
        {
            var company = _dbContext.Users
                                .Include(u => u.Company)
                                .FirstOrDefault(u => u.ID == userId)
                                ?.Company;

            if (company == null)
            {
                // 这可能意味着没有找到与userId关联的Company
                // 在这种情况下，你可能需要返回一个错误信息，而不是继续执行后面的代码
                return null;
            }

            var matches = _dbContext.JobPositions
                                    .Where(jp => jp.CompanyID == company.ID && jp.ID == jobId)
                                    .Include(jp => jp.Resumes)
                                        .ThenInclude(r => r.Applicant)
                                        .ThenInclude(a => a.ApplicantProfile)
                                        .ThenInclude(ap => ap.JobMatches)
                                    .SelectMany(jp => jp.Resumes.Select(r => new { Resume = r, JobMatches = r.Applicant.ApplicantProfile.JobMatches }))
                                    .ToList() // Bring data into memory
                                    .Select(x => new
                                    {
                                        ResumeId = x.Resume.ID,
                                        BestMatch = x.JobMatches.OrderByDescending(jm => jm.Score).FirstOrDefault() // Now it's okay to use FirstOrDefault
                                    })
                                    .Where(x => x.BestMatch != null) // Filter out resumes with no job matches
                                    .Select(x => new ResumeMatch
                                    {
                                        ResumeId = x.ResumeId,
                                        Score = x.BestMatch.Score,
                                        MatchReason = x.BestMatch.Reason
                                    });

            return new JobMatchResultModelClass { Matches = matches.ToList() };
        }

        public AllJobInfoResultClass forAllResumeName(int userId)
        {
            var company = _dbContext.Users
                            .Include(u => u.Company)
                            .FirstOrDefault(u => u.ID == userId)
                            ?.Company;

            if (company == null)
            {
                // 这可能意味着没有找到与userId关联的Company
                // 在这种情况下，你可能需要返回一个错误信息，而不是继续执行后面的代码
            }

            var jobInfos = _dbContext.JobPositions
                                     .Where(jp => jp.CompanyID == company.ID)
                                     .Select(jp => new OneJobName
                                     {
                                         Id = jp.ID,
                                         JobName = jp.Title
                                     })
                                     .ToList();

            return new AllJobInfoResultClass { AllJobNames = jobInfos };
        }
    }
}
