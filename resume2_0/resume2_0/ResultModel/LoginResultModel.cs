namespace resume2_0.ResultModel
{
    public class LoginResultModel
    {
        public string Data { get; set;  } //admin-token editor-yoken
        public int Code { get; set;  }  //为20000, 表示登录成功 60204，表示登录失败
        public int UserId { get; set;} //用来表示的用户ID，后续查询数据库时候要用

    }
}
