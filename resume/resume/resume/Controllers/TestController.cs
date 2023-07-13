/*using Microsoft.AspNetCore.Mvc;
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
            string filePath = @"D:\PythonCode\openai\txt";
            string dataFilePath = @"D:\visualStudio workspace\SCB_ClassAssignment_backend\end\json\1.json"; // 数据文件路径
            Dictionary<string, object> resumeInfo = null;
                // 从文件中读取数据
            string resumeData = System.IO.File.ReadAllText(dataFilePath);
                resumeInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(resumeData);
                // 传入参数：filepath 返回：FirstAddResumeModelClass 并实现将该路径存入数据库
            var storedApplicant = _applicantService.CreateApplicantFromDictionary(resumeInfo);
            Applicant applicantResult = storedApplicant.Result;
            int resumeID = _resumeService.AddResumePath(filePath, applicantResult, 1 , 1);
                var detailedResume = new DetailedResume
                {
                    Id = applicantResult.ID,
                    Age = applicantResult.Age,
                    Name = applicantResult.Name,
                    Email = applicantResult.Email,
                    PhoneNumber = applicantResult.PhoneNumber,
                    JobIntention = "tobefinished",
                    Gender = applicantResult.Gender,
                    SelfEvaluation = applicantResult.SelfEvaluation,
                    HighestEducation = applicantResult.HighestEducation,
                    //TalentTraits = applicantResult.ApplicantProfile.TalentTraits,
                    Awards = applicantResult.Awards?.Select(a => new AwardInfo { AwardName = a.AwardName }).ToList() ?? new List<AwardInfo>(), // assign Awards
                    WorkExperience = applicantResult.WorkExperiences?.ToList() ?? new List<WorkExperience>(),  // assign WorkExperiences
                    SkillCertificate = applicantResult.SkillCertificates?.ToList() ?? new List<SkillCertificate>(),  // assign SkillCertificates
                    WorkStability = applicantResult.ApplicantProfile.WorkStability,
                    WorkStabilityReason = applicantResult.ApplicantProfile.StabilityReason,
                    MatchingScore = applicantResult.ApplicantProfile.MatchingScore,
                    MatchingReason = applicantResult.ApplicantProfile.MatchingReason,
                    EducationBackgrounds = applicantResult.EducationBackgrounds?.ToList() ?? new List<EducationBackground>()
                };
                var result = new FirstAddResumeModelClass()
                {
                    Code = 20000,
                    DetailedResume = detailedResume
                };
                return result;
            }

    }
}
*/