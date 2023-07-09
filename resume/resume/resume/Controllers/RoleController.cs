using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using resume.Others;
using resume.ResultModels;
using resume.Services;
using resume.WebSentModel;

namespace resume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly CompanyService _companyService;
        public RoleController(CompanyService companyService)
        {
            _companyService = companyService;
        }
        /// <summary>
        /// 这个接口用于创建新的用户。接口需要接受的数据包括：
        ///- `username`：用户名，字符串类型。
        ///- `password`：用户密码，字符串类型。
        ///- `role`：用户角色，字符串类型
        /// </summary>
        /// <param name="NewUserInfo"></param>
        /// <returns></returns>
        [HttpPost("createUser")]
        public CreateUserResultClass CreateUser(CreateUserSentClass NewUserInfo) 
        {
            //将该新的用户存入数据库，并且注意公司的号
            var result = _companyService.CreateUserByClass(NewUserInfo);
            return result;
        }

        /// <summary>
        /// 该接口是用来删除对应的ID的用户的
        /// </summary>
        /// <param name="deleteUserSent"></param>
        /// <returns></returns>
        [HttpPost("deleteUser")]
        public DeleteUserResultClass DeleteUser(DeleteUserSentClass deleteUserSent) 
        {
            var result = _companyService.DeleteUser(deleteUserSent);
            //需要将该ID对应的用户删除
            return result;
        }



        /// <summary>
        /// 返回该管理ID所对应的所有的用户账户的信息
        /// </summary>
        /// <param name="webSentUserId"></param>
        /// <returns></returns>
        [HttpPost("allUsers")]
        public AllUserResultClass ForAllUsers(WebSentUserId webSentUserId)
        {
            int userId=webSentUserId.Id;
            //返回该公司ID所管理的所有简历的信息
            var result = _companyService.ForAllUsers(userId);
            return result;
        }

    }

}
