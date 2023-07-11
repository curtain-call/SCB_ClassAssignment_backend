namespace resume.WebSentModel
{
    /// <summary>
    /// 上传的所创建的信息
    /// </summary>
    public class CreateUserSentClass
    {
        public int Id { get; set; }//这是公司的ID，也就是管理者的ID
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
