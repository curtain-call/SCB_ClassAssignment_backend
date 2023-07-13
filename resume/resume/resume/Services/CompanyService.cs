using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Security;
using resume.Controllers;
using resume.Models;
using resume.Others;
using resume.ResultModels;
using resume.WebSentModel;
using System.ComponentModel.DataAnnotations;

namespace resume.Services
{
    public class CompanyService
    {
        private readonly MyDbContext _dbContext;

        public CompanyService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Company GetCompanyById(int id)
        {
            return _dbContext.Companies.FirstOrDefault(c => c.ID == id);
        }

        public IEnumerable<JobPosition> GetJobPositionsByCompanyId(int companyId)
        {
            return _dbContext.JobPositions.Where(jp => jp.CompanyID == companyId).ToList();
        }

        // Add more methods for querying the Companies table as needed
        public LoginModelClass IsLogin(string account, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(c => c.Account == account && c.Password == password);

            var model = new LoginModelClass();
            if (user != null)
            {
                model.Code = 20000;
                model.UserId = user.ID;
                model.Data = user.Role;
            }
            else
            {
                model.Code = 60204;
            }
            return model;
        }

        public IEnumerable<ResumeDTO> SearchByCompanyId(int companyId)
        {
            var resumes = _dbContext.Resumes.Include(r => r.Applicant)
                                 .ThenInclude(a => a.ApplicantProfile)
                                 .Include(r => r.JobPosition)
                                 .Where(r => r.JobPosition.CompanyID == companyId)
                                 .Select(r => new ResumeDTO
                                 {
                                     ResumeId = r.ID,
                                     ApplicantName = r.Applicant.Name,
                                     JobPosition = r.JobPosition.Title,
                                     MatchingScore = r.Applicant.ApplicantProfile.MatchingScore
                                 })
                                 .ToList();
            return resumes;
        }

        public RegisterModelClass CreateNewAccount(RegisterSentModel register)
        {
            var company = _dbContext.Companies.FirstOrDefault(c => c.Name == register.Name);
            var user = _dbContext.Users.FirstOrDefault(u => u.Account ==  register.Account);
            var model = new RegisterModelClass();

            if (company != null)
            {
                // 公司已经存在
                model.IsSuccess = false;
                model.Msg = "Company already exists.";
            }
            else if (user != null){
                // 账号已经存在
                model.IsSuccess = false;
                model.Msg = "Account already exists.";
            }
            else
            {
                // 添加新的公司
                company = new Company
                {
                    Name = register.Name,
                };
                _dbContext.Companies.Add(company);
                _dbContext.SaveChanges();

                user = new Models.User
                {   
                    CompanyID = company.ID,
                    Account = register.Account,
                    Email = register.Email,
                    Password = register.Password,
                    Role = "admin",
                    Company = company,
                };

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                model.IsSuccess = true;
                model.Msg = "Registration successful.";
            }

            return model;
        }

        public InfoForHomeModelClass GetInfoForHome(int userId)
        {
            // 通过userId查询Company
            var company = _dbContext.Companies.Include(c => c.Users)
                                            .Include(u => u.Resumes)
                                            .Include(c => c.JobPositions)
                                            .FirstOrDefault(c => c.Users.Any(u => u.ID == userId));

            // 如果找不到Company，返回空值或错误
            if (company == null)
            {
                return new InfoForHomeModelClass(); // 或者其他的错误处理代码
            }

            var infoForHome = new InfoForHomeModelClass
            {
                // 总简历数
                totalResumes = company.Resumes.Count(),
                // 总岗位数
                totalJobs = company.JobPositions.Count,
                // 每天新增的岗位以及简历数
                weeklyStates = GetWeeklyStates(userId),
                // 显示在该主页的所需的简历历史信息
                resumeHistory = GetResumeHistory(company),
                // 岗位对应的简历数量的数组
                JobResumeCounts = GetJobResumeCounts(company)
            };

            return infoForHome;
        }

        private HomeWeeklyState GetWeeklyStates(int userId)
        {
            var user = _dbContext.Users.Find(userId);

            // 检查找到的用户是否为空，并获取其公司ID
            var companyId = user != null ? user.CompanyID : 0;


            // 获取过去七天的日期列表，包含今天
            var dateList = Enumerable.Range(0, 7).Select(days => DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd")).ToList();

            var allJobPositions = _dbContext.JobPositions.AsEnumerable().Where(jp => jp.CompanyID == companyId).ToList();
            var allResumes = _dbContext.Resumes.AsEnumerable().Where(r => r.CompanyID == companyId).ToList();

            var weeklyStates = new HomeWeeklyState
            {
                JobCounts = dateList.Select(date => new HomeJobCount
                {
                    Date = date,
                    Count = allJobPositions
                        .Where(jp => jp.CreatedDate.ToString("yyyy-MM-dd") == date)
                        .Count()
                }).ToList(),

                resumeCounts = dateList.Select(date => new HomeResumeCount
                {
                    Date = date,
                    Count = allResumes
                        .Where(r => r.CreatedDate.ToString("yyyy-MM-dd") == date)
                        .Count()
                }).ToList(),
            };

            return weeklyStates;
        }


        private List<BriefHomeResumeInfo> GetResumeHistory(Company company)
        {
            // 这里编写获取显示在主页所需的简历历史信息的代码
            var history = _dbContext.Resumes
                                    .Include(r => r.Applicant)
                                    .Include(r => r.JobPosition)
                                    .Where(r => r.CompanyID == company.ID)
                                    .Select(r => new BriefHomeResumeInfo
                                    {
                                        RId = r.ID,
                                        ResumeName = r.Applicant.Name,
                                        JobIntention = r.JobPosition.Title,
                                        UploadDate = r.CreatedDate.ToString("yyyy-MM-dd")
                                    })
                                    .ToList();

            return history;
        }

        private List<JobResumeCount> GetJobResumeCounts(Company company)
        {
            // 这里编写获取岗位对应的简历数量的代码
            var jobResumeCounts = _dbContext.JobPositions
                                            .Where(jp => jp.CompanyID == company.ID)
                                            .Select(jp => new JobResumeCount
                                            {
                                                JobName = jp.Title,
                                                ResumeCount = _dbContext.Resumes.Count(r => r.JobPositionID == jp.ID)
                                            })
                                            .ToList();


            return jobResumeCounts;
        }

        public CreateUserResultClass CreateUserByClass(CreateUserSentClass NewUserInfo)
        {
            var result = new CreateUserResultClass();

            // 先检查公司是否存在
            var company = _dbContext.Companies.FirstOrDefault(c => c.ID == NewUserInfo.CompanyID);
            if (company == null)
            {
                result.Code = 400;
                result.Message = "Company does not exist.";
                return result;
            }

            // 再检查账号是否已经存在
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Account == NewUserInfo.Account);
            if (existingUser != null)
            {
                result.Code = 400;
                result.Message = "Account already exists.";
                return result;
            }

            // 如果没有问题，就添加新的用户
            var user = new Models.User
            {
                CompanyID = NewUserInfo.CompanyID,
                Account = NewUserInfo.Account,
                Email = NewUserInfo.Email,
                Password = NewUserInfo.Password,
                Role = NewUserInfo.Role,
                
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            result.Code = 200;
            result.Message = "User created successfully.";
            return result;
        }
        
        public DeleteUserResultClass DeleteUser(DeleteUserSentClass deleteUserSent)
        {
            var result = new DeleteUserResultClass();

            // 首先查找该用户是否存在
            var user = _dbContext.Users.FirstOrDefault(u => u.ID == deleteUserSent.Id);

            if (user == null)
            {
                // 如果用户不存在，返回错误信息
                result.Code = 400;
                result.Message = "User not found.";
                return result;
            }

            // 删除用户
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            // 返回成功信息
            result.Code = 200;
            result.Message = "User deleted successfully.";
            return result;
        }

        public AllUserResultClass ForAllUsers(int userId)
        {

            // 查找当前用户对应的公司
            var company = _dbContext.Users
                                    .Include(u => u.Company)
                                    .FirstOrDefault(u => u.ID == userId)
                                    ?.Company;

            if (company == null)
            {
                // 如果找不到对应的公司，返回错误信息
                return new AllUserResultClass { Users = new List<ResultModels.BriefUser>() };
            }

            // 获取该公司下所有用户的信息
            var users = _dbContext.Users
                                  .Where(u => u.CompanyID == company.ID)
                                  .Select(u => new ResultModels.BriefUser { Id = u.ID, Account = u.Account, Role = u.Role })
                                  .ToList();

            return new AllUserResultClass { Users = users };    
        }

        public HomeToUploadResume UploadJobInfo(int userId)
        {
            // 查找当前用户对应的公司
            var company = _dbContext.Users
                                    .Include(u => u.Company)
                                    .FirstOrDefault(u => u.ID == userId)
                                    ?.Company;

            if (company == null)
            {
                // 如果找不到对应的公司，返回错误信息
                return new HomeToUploadResume { UploadNeedJobsInfo = new List<JobInfoForUpload>() };
            }

            // 获取该公司下所有岗位的信息
            var jobsInfo = _dbContext.JobPositions
                                     .Where(jp => jp.CompanyID == company.ID)
                                     .Select(jp => new JobInfoForUpload { JobId = jp.ID, JobName = jp.Title })
                                     .ToList();

            return new HomeToUploadResume { UploadNeedJobsInfo = jobsInfo };
        }
    }
}
