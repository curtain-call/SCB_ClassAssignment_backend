namespace resume.ResultModels
{
    public class CreateUserResultClass
    {
        public int Code { get; set; }//状态码标识添加成功还是失败？
        public string Message { get; set; }//信息：失败的原因
    }
}
