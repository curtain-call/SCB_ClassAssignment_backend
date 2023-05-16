using System.Collections.Generic;
using System.Linq;

namespace IntelligentResumeParsingSystem
{
    public class JobPositionService
    {
        private readonly JobPositionRepository _jobPositionRepository;

        public JobPositionService(JobPositionRepository jobPositionRepository)
        {
            _jobPositionRepository = jobPositionRepository;
        }

        public void CreateJobPosition(JobPosition jobPosition)
        {
            _jobPositionRepository.Save(jobPosition);
        }

        public void UpdateJobPosition(JobPosition jobPosition)
        {
            _jobPositionRepository.Update(jobPosition);
        }

        public void DeleteJobPosition(int jobPositionId)
        {
            _jobPositionRepository.Delete(jobPositionId);
        }

        public JobPosition GetJobPositionById(int jobPositionId)
        {
            return _jobPositionRepository.FindById(jobPositionId);
        }

        public List<JobPosition> GetJobPositions()
        {
            return _jobPositionRepository.FindAll().ToList();
        }
    }
}
