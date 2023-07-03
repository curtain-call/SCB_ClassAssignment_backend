namespace resume.ResultModels
{
    /// <summary>
    /// 该公司的，该岗位的人岗匹配结果
    /// </summary>
    public class JobMatchResultModelClass
    {
        public List<ResumeMatch> Matches { get; set; }

    }
    public class ResumeMatch
    {
        public int ResumeId { get; set; }
        public int Score { get; set; }
        public string MatchReason { get; set; }
    }

}
