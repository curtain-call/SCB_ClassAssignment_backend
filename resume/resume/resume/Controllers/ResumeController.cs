using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using resume.Others;
using resume.WebSentModel;
using resume.ResultModels;
using System.Security.Cryptography;
using resume.Services;
using resume.Models;
using resume.Service;
using resume.open;
using Microsoft.AspNetCore.StaticFiles;

namespace resume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        private readonly CompanyService _companyService;
        private readonly ResumeService _resumeService;
        private readonly ApplicantService _applicantService;
        public ResumeController(CompanyService companyService, ResumeService resumeService, ApplicantService applicantService)
        {
            _companyService = companyService;
            _resumeService = resumeService;
            _applicantService = applicantService;
        }
        /// <summary>
        /// 此时，点击上传简历页面时，需要我们返回岗位下拉框里面的所有内容
        /// </summary>
        /// <param name="webSentUserId"></param>
        /// <returns></returns>
        /// 
        [HttpPost("first")]
        public HomeToUploadResume UploadJobInfo(WebSentUserId webSentUserId)
        {
            int userId = webSentUserId.Id;//用户ID
            //此时，需要完成的是，查数据库返回所有岗位的名称以及ID；
            var result = _companyService.UploadJobInfo(userId);
            return result;
        }

        [HttpPost("upload")]
        public FirstAddResumeModelClass UploadResume(IFormFile file, int jobId, int UserId)
        {

            //将文件存在指定的地点
            //并获得文件的存储地址
            string fileName = Path.GetFileName(file.FileName);
            string staticFileRoot = "Resumes";
            //这里是文件路径，不包含文件名
            string fileUrlWithoutFileName = @$"{DateTime.Now.Year}\{DateTime.Now.Month}\{DateTime.Now.Day}";
            //创建文件，如过文件已经存在，则什么也不做
            Directory.CreateDirectory($"{staticFileRoot}/{fileUrlWithoutFileName}");

            //使用哈希的原因是前端可能传递相同的文件，服务端不想保存多个相同的文件

            SHA256 hash = SHA256.Create();
            //读取文件刘的值 把文件流转换为哈希值
            byte[] hashByte = hash.ComputeHash(file.OpenReadStream());
            //在把哈希值转为字符串 当作文件的文件名
            string hashedFileName = BitConverter.ToString(hashByte).Replace("-", "");

            //重新获得一个文件名
            string newFileName = hashedFileName + "." + fileName.Split('.').Last();
            //应该创建的文件的绝对地址
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $@"{staticFileRoot}\{fileUrlWithoutFileName}", newFileName);
            //创建这个文件，依据绝对地址
            using var fileStream = new FileStream(filePath, FileMode.Create);
            //将该网络传来的文件，全部都赋值给那个新建的文件
            file.CopyTo(fileStream);
            Console.WriteLine(filePath);
            //接下来就是调用算法分析简历，返回对应的简历信息
            Connect connect = new Connect();
            var resumeInfo = connect.analysis(filePath);
            //string filePathTest = @"D:\visualStudio workspace\SCB_ClassAssignment_backend\end\word\1.docx";
            //Dictionary<string, object> resumeInfo = connect.analysis(filePathTest);
            Console.WriteLine(resumeInfo);  
 
            var storedApplicant = _applicantService.CreateApplicantFromDictionary(resumeInfo);
            Applicant applicantResult = storedApplicant.Result;
            int resumeID = _resumeService.AddResumePath(filePath, applicantResult, UserId, jobId);
            var detailedResume = new DetailedResume
            {
                Id = applicantResult.ID,
                Age = applicantResult.Age,
                Name = applicantResult.Name,
                Email = applicantResult.Email,
                PhoneNumber = applicantResult.PhoneNumber,
                JobIntention = applicantResult.JobIntention,
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
            var result = new FirstAddResumeModelClass() { 
                Code = 20000,
                DetailedResume = detailedResume
            };  
            return result;
        }

        [HttpGet("getGraph")]
        public IActionResult GetFile(int resumeId)
        {
            Console.WriteLine(resumeId);
            string imagePath = _resumeService.GetImagePathById(resumeId);
            //string filePath = "C:\\Users\\86178\\Desktop\\大学本科学习资料\\大二下\\中国软件杯\\resume\\resume\\Resumes\\2023\\7\\11\\C5C5FA6B19833453071A6E76E1836D835D555FFF13388E450DCA7FE55F82040E.jpg"; // 文件路径
            //
            FileStream fileStream = new FileStream(imagePath, FileMode.Open);

            string mimeType;
            new FileExtensionContentTypeProvider().TryGetContentType(imagePath, out mimeType); // 获取文件的MIME类型

            return new FileStreamResult(fileStream, mimeType); // 设置 MIME 类型为二进制流
        }

        /// 修改简历（左右两侧，左侧简历，右侧修改确认）
        [HttpPost("SecondUpload")]
        public SecondAddResumeModelClass SencondUploadResume(FirstAddResumeModelClass firstAddResume) {
            //此时将修改后的数据，上传到数据库
            //并返回是否添加成功的状态码。
            DetailedResume detailedResume = firstAddResume.DetailedResume;
            var result = new SecondAddResumeModelClass();
            bool status = _applicantService.UpdateApplicant(detailedResume);
            if (status)
            {
                result.Code = 20000;
            } else { result.Code = 60204;  }
            return result;
        }

        /// <summary>
        /// 所有简历，列表展示
        /// </summary>
        /// <param name="webSentUserId"></param>
        /// <returns></returns>
        [HttpPost("AllSimpleReusmes")]
        public AllSimpleResumeInfoClass ForAllSimpleResumeInfo(WebSentUserId webSentUserId)
        {
            int id = webSentUserId.Id;
            //此时查数据库，获得该用户所在公司的所有简历的简单信息
            var result = _applicantService.ForAllSimpleResumes(id);
            return result;
        
        }

        /// <summary>
        /// 展示的是一个简历的所有详细信息
        /// </summary>
        /// <param name="resumeId"></param>
        /// <returns></returns>
        [HttpPost("OneDetailedResumeInfo")]
        public DetailedResume ForOneDetailedResumeInfo(WebSentUserId resumeId) { 
            int id = resumeId.Id;//此时的ID便是该简历的resumeID；
            var result = _resumeService.GetResumeById(id);
            return result;
        }

        ///-------------以下所有的接口都是为了简历统计可视化中的图----------------

        ///简历总数（类似与外卖那个首页的总数）
        ///每个岗位多少人：简历总数，每个岗位人数（瀑布图）
        ///传入
        //{
        //  userid:
        //}
        //    返回：
        //{
        //  "totalResumes": 500,
        //  "jobResumeCounts": [
        //  {"jobName": "jobName", "resumeCount": 10},
        //  { "jobName": "jobName", "resumeCount": 20},
        //  ]
        //}
        [HttpPost("graph/total")]
        public GraphForJonResumeCountModelClass ForJonResumeCount(WebSentUserId webSentUserId)
        {
            int userId = webSentUserId.Id;//这是用户ID
            var result = _resumeService.ForJonResumeCount(userId);
            return result;
        }

        ///最高学历：高中及以下，大专，本科，硕士，博士
        ///毕业院校：985/211，普通一本，二本及以下
        ///（以上两个用嵌套环形图）
        [HttpPost("graph/education")]
        public EducationInfoForGraphClass ForGraphByEducation(WebSentUserId webSentUserId)
        {
            int userId = webSentUserId.Id;
            var result = _resumeService.ForGraphByEducation(userId);
            //查出该ID对应的关于学历的所有简历数量分布
            return result;
        }

        //年龄段：18-22，22-25，25-30，30-35，35以上（柱状图，横坐标年龄段，纵坐标个数）
        [HttpPost("graph/ageInfo")]
        public AgeInfoForGraphClass ageInfoForGraphClass(WebSentUserId webSentUserId)
        {
            int userId = webSentUserId.Id;
            //查出该ID所对饮的简历数量
            var result = _resumeService.AgeInfoForGraphClass(userId);
            return result;
        }

        /// <summary>
        /// 工作年限：0，1，2，3，4，5，6···14，15，15以上（大数据量柱图）
        /// </summary>
        /// <param name="webSentUserId"></param>
        /// <returns></returns>
        [HttpPost("graph/workYears")]
        public WorkYearInfoForGraphClass workYearInfoForGraph(WebSentUserId webSentUserId)
        {
            int userId = webSentUserId.Id;
            //查出该用户所在公司的各年龄端的简历数量
            var result = _resumeService.WorkYearInfoForGraph(userId);
            return result;
        }

        /// <summary>
        /// 工作稳定性；下，中下，中，中上，上（半环形图）
        /// </summary>
        /// <param name="webSentUserId"></param>
        /// <returns></returns>
        [HttpPost("graph/workStability")]
        public WorkStabilityInfoForGraphClass WorkStabilityInfoForGraph(WebSentUserId webSentUserId)
        {
            int userId = webSentUserId.Id;
            //查出该用户所在公司的所有简历的工作稳定性数量
            var result = _resumeService.WorkStabilityInfoForGraph(userId);
            return result;
        }

        /// <summary>
        /// 人岗匹配程度得分：60以下，60-70，70-80，80-90，90-100（南丁格尔玫瑰图）
        /// </summary>
        /// <param name="webSentUserId"></param>
        /// <returns></returns>
        [HttpPost("graph/JobMatchScore")]
        public JobMatchScoresInfoForGraphClass JobMatchScoresInfoForGraph(WebSentUserId webSentUserId)
        {
            int id = webSentUserId.Id;
            //此时查数据库，查出该用户所在公司的所有简历分数排布
            var result = _applicantService.JobMatchScoresInfoForGraph(webSentUserId.Id);
            return result;
        }

        [HttpPost("graph/PersonalJobMatchScore")]
        public ICollection<JobMatch> GetJobMatches(WebSentResumeId webSentResumeId)
        {
            var result = _resumeService.GetJobMatchesByResumeId(webSentResumeId.Id);
            return result;
        }

        //简历详情页可以获得这个人的匹配分数
        [HttpPost("graph/PersonalCharacteristics")]
        public PersonalCharacteristics GetPersonalCharacteristics(WebSentResumeId webSentResumeId)
        {
            var result = _applicantService.GetPersonalCharacteristics(webSentResumeId.Id);
            return result;
        }

        [HttpPost("graph/SkillsAndExperiences")]
        public SkillsAndExperiences GetSkillsAndExperiences(WebSentResumeId webSentResumeId)
        {
            var result = _applicantService.GetSkillsAndExperiences(webSentResumeId.Id);
            return result;
        }

        [HttpPost("graph/AchievementsAndHighlights")]
        public AchievementsAndHighlights GetAchievementsAndHighlights(WebSentResumeId webSentResumeId)
        {
            var result = _applicantService.GetAchievementsAndHighlights(webSentResumeId.Id);
            return result;
        }



    }
}
