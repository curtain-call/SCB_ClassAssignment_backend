using System.Collections.Generic;
using System.Linq;

namespace IntelligentResumeParsingSystem.Services
{
    public class ResumeService
    {
        private readonly ResumeRepository _resumeRepository;

        public ResumeService(ResumeRepository resumeRepository)
        {
            _resumeRepository = resumeRepository;
        }

        public void CreateResume(Resume resume)
        {
            _resumeRepository.Save(resume);
        }

        public void UpdateResume(Resume resume)
        {
            _resumeRepository.Update(resume);
        }

        public void DeleteResume(int resumeId)
        {
            _resumeRepository.Delete(resumeId);
        }

        public Resume GetResumeById(int resumeId)
        {
            return _resumeRepository.FindById(resumeId);
        }

        public List<Resume> GetResumes()
        {
            return _resumeRepository.FindAll().ToList();
        }
    }
}
