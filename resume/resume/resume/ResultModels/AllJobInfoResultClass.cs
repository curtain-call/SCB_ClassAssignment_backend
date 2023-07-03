namespace resume.ResultModels
{
    /// <summary>
    /// 返回该公司对应的所有岗位名字以及岗位ID
    /// </summary>
    public class AllJobInfoResultClass
    {
        
       public List<OneJobName> AllJobNames { get; set; }
    }

    public class OneJobName
    {
        public int Id { get; set; }//岗位ID
        public string JobName { get; set; }//岗位名字
    }

}