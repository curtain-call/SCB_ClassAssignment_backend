namespace resume.WebSentModel
{
    /// <summary>
    /// 上传的所创建的信息
    /// </summary>
    public class CreateUserSentClass
    {
        public int CompanyID { get; set; }
        public string Account { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }  // 用户的角色，例如 "admin", "normal" 等
    }
}
