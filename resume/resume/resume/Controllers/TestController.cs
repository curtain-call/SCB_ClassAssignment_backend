using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using resume.Models;
using resume.open;
using resume.Others;
using resume.ResultModels;
using resume.Service;
using resume.Services;

namespace resume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ApplicantService _applicantService;
        private readonly ResumeService _resumeService;
        public TestController(ApplicantService applicantService, ResumeService resumeService)
        {
            _applicantService = applicantService;
            _resumeService = resumeService;
        }
        [HttpPost]
        [Route("AnalysisTest")]
        public FirstAddResumeModelClass AnalysisTest()
        {
            int UserId = 1;
            int jobId = 1;
            string filePath = @"D:\PythonCode\openai\txt";
            string filePath_1 = @"D:\Test";
            string dataFilePath = @"D:\visualStudio workspace\SCB_ClassAssignment_backend\json\200.json"; // 数据文件路径
            Dictionary<string, object> resumeInfo = null;
            // 从文件中读取数据
            string resumeData = System.IO.File.ReadAllText(dataFilePath);
            resumeInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(resumeData);
            // 传入参数：filepath 返回：FirstAddResumeModelClass 并实现将该路径存入数据库
            var storedApplicantId = _applicantService.CreateApplicantFromDictionary(resumeInfo).Result;

            int resumeID = _resumeService.AddResumePath(filePath, filePath_1, storedApplicantId, UserId, jobId);
            var detailedResume = _resumeService.GetResumeById(resumeID);
            var result = new FirstAddResumeModelClass()
            {
                Code = 20000,
                DetailedResume = detailedResume
            };
            return result;
        }

    }
}
