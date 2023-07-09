namespace resume.Others
{
    /// <summary>
    /// 上传的所有简历的分布（以岗位名称）
    /// </summary>
    public class JobResumeCount
    {
        public string? JobName { get; set; }//岗位名称
        public int ResumeCount { get; set; }//该公司的该岗位的简历数量
    }

}
