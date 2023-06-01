using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using resume2_0.WebSentModel;
using resume2_0.ResultModel;

namespace resume2_0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// 登录界面 前端发送过来账号和密码 后端返回一些值（判断登录是否成功）
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public LoginResultModel  Login(LoginSentModel login) { 
            string password=login.Password;
            string userName=login.UserName;
            //调用接口 查看数据库 看登录是否成功
            //传入 ：password userName 
            // 返回类LoginResultModel ,具体赋值 详见LoginResultModel类

            //测试 Login
            LoginResultModel loginResultModel = new LoginResultModel();
            loginResultModel.Code = 520;
            loginResultModel.UserId = 250;
            loginResultModel.Data = "hah";
            return loginResultModel;


        
        }
    }
}
