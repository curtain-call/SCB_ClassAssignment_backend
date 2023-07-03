using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using resume.ResultModels;
using resume.WebSentModel;
using resume.Others;



namespace resume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [Route("Login")]
        [HttpPost]
        public LoginModelClass Login(LoginSentModel loginSentModel) { 
            
            string userName=loginSentModel.UserName;
            string password=loginSentModel.Password;

            //此时应当完成，查数据库，判断该用户是否存在，以及返回对应的信息(也就是LoginModelClass类，详情见该类的注释)

           LoginModelClass re= new LoginModelClass();
            return re;
        }

        [HttpPost("Register")]
        public RegisterModelClass Register(RegisterSentModel registerSentModel) {
            
            
            return new RegisterModelClass(); }



    }

    
  




}
