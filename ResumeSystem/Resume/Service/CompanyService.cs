using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ResumeSystem.Models;
using ResumeSystem.WebSentModels;

namespace ResumeSystem.Services
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
        public LoginResultModel IsLogin(string account, string password)
        {
            var company = _dbContext.Companies.FirstOrDefault(c => c.Account == account && c.Password == password);

            var model = new LoginResultModel();
            if (company != null ) {
                model.Code = 20000;
                model.UserId = company.ID;
            } else
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

        public RegisterModelClass CreateNewAccount(RegisterSentModels register)
        {
            var company = _dbContext.Companies.FirstOrDefault(c => c.Account == register.Account);
            var model = new RegisterModelClass();

            if (company != null)
            {
                // 账号已经存在
                model.IsSuccess = false;
                model.Msg = "Account already exists.";
            }
            else
            {
                // 添加新的公司
                company = new Company
                {
                    Account = register.Account,
                    Email = register.Email,
                    Password = register.Password
                };
                _dbContext.Companies.Add(company);
                _dbContext.SaveChanges();

                model.IsSuccess = true;
                model.Msg = "Registration successful.";
            }

            return model;
        }
    }
}
