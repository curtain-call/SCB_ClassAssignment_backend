using ResumeSystem.Class;

namespace Intelligent_resume_analysis_system.ResultModels
{
    public class MainToJobResponse
    {
        public List<string> JobNames { get; set; }
        public string FirstJobRequirement { get; set; }
        public List<Resume> SortedResumes { get; set; }
    }
}
