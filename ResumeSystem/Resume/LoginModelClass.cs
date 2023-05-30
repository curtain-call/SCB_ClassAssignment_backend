namespace ResumeSystem.Models
{
    /// <summary>
    /// 登录界面 前端传来数据后，此时查找数据库，返回的结果
    /// </summary>
    public class LoginModelClass
    {
        public bool IsLogin { get; set; }//为1,说明账号存在，为0，则说明账号密码错误
    }
}
