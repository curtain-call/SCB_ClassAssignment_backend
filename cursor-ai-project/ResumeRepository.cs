using System.Collections.Generic;
using System.Linq;

namespace IntelligentResumeParsingSystem.Repositories
{
    public class ResumeRepository
    {
        private readonly ApplicationDbContext _context;

        public ResumeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Save(Resume resume)
        {
            _context.Resumes.Add(resume);
            _context.SaveChanges();
        }

        public void Update(Resume resume)
        {
            _context.Resumes.Update(resume);
            _context.SaveChanges();
        }

        public void Delete(int resumeId)
        {
            var resume = _context.Resumes.FirstOrDefault(r => r.Id == resumeId);
            if (resume != null)
            {
                _context.Resumes.Remove(resume);
                _context.SaveChanges();
            }
        }

        public Resume FindById(int resumeId)
        {
            return _context.Resumes.FirstOrDefault(r => r.Id == resumeId);
        }

        public List<Resume> FindAll()
        {
            return _context.Resumes.ToList();
        }
    }
}
