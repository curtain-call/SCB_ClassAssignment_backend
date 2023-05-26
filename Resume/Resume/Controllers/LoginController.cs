using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resume.Models;
using Resume.WebSentModels;
namespace Resume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        /// <summary>
        /// 获取的是 前端传入的登录信息 返回是否登录成功的信号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public LoginModelClass Login(string account , string password)
        {
            //接下来调用函数  isLogin(string account,string password)(此时需要查数据库,返回值为LoginModelClass)

            //下面这些在调用算法后删除
            LoginModelClass model = new LoginModelClass();
            model.IsLogin = true;
            return model;

        }

        /// <summary>
        /// 实现的是 获取前端传入的注册信息。返回是否注册成功的信号
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public RegisterModelClass Register(RegisterSentModels register)
        {
            //此时需要调用后台端 然后进行数据库匹配（看账号是否已经存在）
            //接口 RegisterModelClass CreateNewAccount(RegisterSentModels register) 
            

            //后面这个是，模拟能否将信息全部传输成功
            RegisterModelClass model = new RegisterModelClass();
            model.Msg ="name:"+ register.Name+";Email:"+register.Email+"; Account:"+register.Account+"; PassWord:"+register.Password;
            model.IsSuccess = true;
            return model;

        }

    }
}
