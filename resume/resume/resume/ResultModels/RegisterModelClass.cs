namespace resume.ResultModels
{
    /// <summary>
    /// 用以注册账号时，返回信息所用
    /// </summary>
    public class RegisterModelClass
    {
        public string Msg { get; set; }//返回注册信息（账号是否已经存在，注册成功）
        public bool IsSuccess { get; set; }//注册成功信号
    }
}
