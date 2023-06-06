using Microsoft.AspNetCore.Mvc;
using ResumeSystem.openai;
using ResumeSystem.ResultModels;
using ResumeSystem.Service;
using ResumeSystem.Services;

namespace ResumeSystem.Controllers
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
        public SimpleResume AnalysisTest()
        {
            string filePath = @"D:\PythonCode\openai\txt";
            Connect connect = new Connect();
            //调用算法接口，对保存的简历进行分析，并将路径保存在数据库中，并返回数据
            Dictionary<string, object> resumeInfo = connect.analysis(filePath,1);

            //传入参数：filepath 返回：FirstAddResumeModelClass 并实现将该路径存入数据库
            var storedApplicant = _applicantService.CreateApplicantFromDictionary(resumeInfo);
            int resumeID = storedApplicant.Result.ID;
            var simpleResume = new SimpleResume
            {
                Rid = resumeID,
                Age = storedApplicant.Result.Age,
                PhoneNumber = storedApplicant.Result.PhoneNumber,
                JobIntention = storedApplicant.Result.JobIntention,
                Gender = storedApplicant.Result.Gender,
                MatChingScore = storedApplicant.Result.ApplicantProfile.MatchingScore,
            };
            _resumeService.UpdateFilePath(filePath, resumeID);
            return simpleResume;
        }

    }
}
