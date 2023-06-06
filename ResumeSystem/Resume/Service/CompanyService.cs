using System.Collections.Generic;
using System.Linq;
using ResumeSystem.Models;

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
    }
}
