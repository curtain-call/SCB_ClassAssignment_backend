using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using resume.ResultModels;
using resume.WebSentModel;
using resume.Others;  
namespace resume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {


        [HttpPost("statistics")]
        public InfoForHomeModelClass  GetInfoForHome(WebSentUserId webSentUserId) {
            int userId = webSentUserId.Id;//前端传来的userID；
            //此时查数据库，返回对应userID所需的主页全部信息;

            return new InfoForHomeModelClass();
        }



    }
}
