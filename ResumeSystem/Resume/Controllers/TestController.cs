using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ResumeSystem.Models;
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
            string dataFilePath = @"D:\PythonCode\openai\AnalysisResult\resumeData99.txt"; // 数据文件路径
            Dictionary<string, object> resumeInfo = null;

            // 判断数据文件是否存在
            if (System.IO.File.Exists(dataFilePath))
            {
                // 从文件中读取数据
                string resumeData = System.IO.File.ReadAllText(dataFilePath);
                resumeInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(resumeData);
            }
            else
            {
                // 调用算法接口，对保存的简历进行分析，并将路径保存在数据库中，并返回数据
                Connect connect = new Connect();
                resumeInfo = connect.analysis(filePath, 99);

                // 将数据写入文件
            string resumeData = JsonConvert.SerializeObject(resumeInfo);
            System.IO.File.WriteAllText(dataFilePath, resumeData);
            }
            // 传入参数：filepath 返回：FirstAddResumeModelClass 并实现将该路径存入数据库
            var storedApplicant = _applicantService.CreateApplicantFromDictionary(resumeInfo);
            Applicant applicantResult = storedApplicant.Result;
            int resumeID = _resumeService.AddResumePath(filePath, applicantResult, 1);
            var simpleResume = new SimpleResume
            {
                Rid = resumeID,
                Age = applicantResult.Age,
                PhoneNumber = applicantResult.PhoneNumber,
                JobIntention = applicantResult.JobIntention,
                Gender = applicantResult.Gender,
                MatChingScore = applicantResult.ApplicantProfile.MatchingScore,
            };

            return simpleResume;
        }

    }
}
