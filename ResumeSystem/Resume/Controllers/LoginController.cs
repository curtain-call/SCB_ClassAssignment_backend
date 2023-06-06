using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeSystem.Models;
using ResumeSystem.Services;
using ResumeSystem.WebSentModels;
namespace ResumeSystem.Controllers
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
        private readonly CompanyService _companyService;
        public LoginController(CompanyService companyService)
        {
            _companyService = companyService;
        }
        [HttpPost]
        [Route("Login")]
        public LoginResultModel  Login(LoginSentModel login) { 
            string password=login.Password;
            string userName=login.UserName;
            //调用接口 查看数据库 看登录是否成功
            //传入 ：password userName 
            // 返回类LoginResultModel ,具体赋值 详见LoginResultModel类
            var result = _companyService.IsLogin(userName, password);
            if (result.Code == 20000) { result.Data = "admin-token"; }
            return result;
        }
    }
}
