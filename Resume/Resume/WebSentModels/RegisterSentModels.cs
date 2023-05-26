namespace Resume.WebSentModels
{
    /// <summary>
    /// 注册时，前端需要传输过来的信号
    /// </summary>
    public class RegisterSentModels
    {
        public int CompanyId { get; set; }//此时，后面用来唯一标识用户的
        public string Account { get; set; }
        public string Password { get; set; }    
        public string  Email{ get; set; }

        public string Name {get; set; }
    }
}
