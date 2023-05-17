using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelligent_resume_analysis_system.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // ...注入UserService等...

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            // ...注册逻辑...
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // ...登录逻辑...
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // ...登出逻辑...
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            // ...获取用户信息逻辑...
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserDto userDto)
        {
            // ...更新用户信息逻辑...
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            // ...重设密码逻辑...
        }
    }

}
