using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace IntelligentResumeParser.Repositories
{
    public class JobPositionRepository : IJobPositionRepository
    {
        private readonly AppDbContext _context;

        public JobPositionRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Save(JobPosition jobPosition)
        {
            _context.JobPositions.Add(jobPosition);
            _context.SaveChanges();
        }

        public void Update(JobPosition jobPosition)
        {
            _context.JobPositions.Update(jobPosition);
            _context.SaveChanges();
        }

        public void Delete(int jobPositionId)
        {
            var jobPosition = _context.JobPositions.Find(jobPositionId);
            if (jobPosition != null)
            {
                _context.JobPositions.Remove(jobPosition);
                _context.SaveChanges();
            }
        }

        public JobPosition FindById(int jobPositionId)
        {
            return _context.JobPositions.Find(jobPositionId);
        }

        public List<JobPosition> FindAll()
        {
            return _context.JobPositions.ToList();
        }
    }
}
