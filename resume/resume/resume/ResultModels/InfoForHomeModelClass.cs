using resume.Others;

namespace resume.ResultModels
{
    /// <summary>
    /// 主页所需的所有信息
    /// </summary>
    public class InfoForHomeModelClass
    {
        public int totalResumes { get; set; }//该公司对应的所有简历数量
        public int totalJobs { get; set; }//所有岗位数量

        public HomeWeeklyState weeklyStates { get; set; }//每天新增德的岗位以及简历数
        public List<BriefHomeResumeInfo> resumeHistory { get; set; }//显示在该主页的所需的简历历史信息
        public List<JobResumeCount> JobResumeCounts { get; set; }//岗位对应的简历数量的数组
    }
}
