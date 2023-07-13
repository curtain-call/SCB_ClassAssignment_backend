using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using resume.ResultModels;
using resume.WebSentModel;
using resume.Services;
namespace resume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly CompanyService _companyService;
        public HomeController(CompanyService companyService)
        { 
            _companyService = companyService;
        }

        [HttpPost("statistics")]
        public InfoForHomeModelClass  GetInfoForHome(WebSentUserId webSentUserId) {
            int userId = webSentUserId.Id;//前端传来的userID；
            //此时查数据库，返回对应userID所需的主页全部信息;
            var result = _companyService.GetInfoForHome(userId);
            return result;
        }
    }
}
