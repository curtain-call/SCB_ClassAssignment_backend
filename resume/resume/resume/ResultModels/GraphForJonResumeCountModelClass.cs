using resume.Others;
namespace resume.ResultModels
{
    /// <summary>
    /// 
    /// </summary>
    public class GraphForJonResumeCountModelClass
    {
        public int TotalResumes { get; set; }//所有简历数
        public List<JobResumeCount> JobResumeCounts { get; set; }
    }


}