namespace resume.ResultModels
{
    /// <summary>
    /// 登录界面 前端传来数据后，此时查找数据库，返回的结果
    /// </summary>
    public class LoginModelClass
    {
        public string? Data { get; set; }//这个用来标识用户权限，为admin则是普通用户，为editor则为创建者用户
        public int Code { get; set; }//登录成功状态码,20000登录成功，60204登录失败
        public int UserId { get; set; }//用户ID
        public int CompanyId { get; set; }

    }
}
