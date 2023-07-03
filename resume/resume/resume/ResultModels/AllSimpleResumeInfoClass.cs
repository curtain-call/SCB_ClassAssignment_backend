using resume.Others;

namespace resume.ResultModels
{

    /// <summary>
    /// 所有简历，列表展示
    /// </summary>
    public class AllSimpleResumeInfoClass
    {
        public int Code { get; set; }
        public List<SimpleResume> SimpleResumes { get; set; }
    }
}
