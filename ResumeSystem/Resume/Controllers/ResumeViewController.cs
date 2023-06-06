using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResumeSystem.ResultModels;
using ResumeSystem.WebSentModel;
using System.Security.Cryptography;
using ResumeSystem.Services;
using System.Threading.Tasks.Dataflow;
using ResumeSystem.Service;
using ResumeSystem.openai;
using ResumeSystem.ResultModel;
using Microsoft.Extensions.Logging;

namespace ResumeSystem.Controllers
{
    /// <summary>
    /// 这个类是，主页的类
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeViewController : ControllerBase
    {
        private readonly ApplicantService _applicantService;
        private readonly ResumeService _resumeService;

        public ResumeViewController(ApplicantService applicantService, ResumeService resumeService)
        {
            _applicantService = applicantService;
            _resumeService = resumeService;
        }

        /// <summary>
        /// 返回的是该用户id下所有简历的简略信息
        /// </summary>
        /// <param name="allResumeSentModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AllResumes")]
        public AllSimpleResumes ForAllResumes(AllResumeSentModel allResumeSentModel)
        {
            int userId = allResumeSentModel.UserId;//前端传来的用户ID
            //查数据库 调用接口
            //传入 userId 返回 AllSimpleResumes
            var result = _applicantService.ForAllSimpleResumes(userId);
            return result;
        }

        /// <summary>
        /// 在简历界面，点击单一简历的处理
        /// </summary>
        /// <param name="resumeId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DetailedResume")]
        public DetailedResume ForOneDetailedResume(ClickOneResumeSentModel ForoneResume)
        {
            int RId = ForoneResume.RId;//该简历的ID
                                       //调用函数，查询对应的简历ID的所有详细信息
                                       //输入 Rid 返回 detailedResume
            var result = _resumeService.GetResumeById(RId);
            return result;
        }


        /// <summary>
        /// 第一次上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="jobs"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadResume")]
        public SimpleResume UploadResume(IFormFile file, int userId)
        {
            //IFormFile file = uploadResumeSentModel.iFormFile;

            ////返回值
            ////先判断file类型，是不是我们需要的文件类型
            //if (!new[] { "application/pdf", "text/plain", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/msword" }.Contains(file.ContentType))
            //{

            //    first.IsSuccess = false;
            //    first.Msg = "仅支持doc，docx，txt，pdf格式文件上传";
            //    return first;
            //}
            Connect connect = new Connect();
            //接着判断文件是否为空
            if (file is { Length: > 0 })
            {//非空
                try
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

                    //调用算法接口，对保存的简历进行分析，并将路径保存在数据库中，并返回数据
                    Dictionary<string, object> resumeInfo = connect.analysis("filePath",1);
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
                catch (Exception ex)
                {
                    // Log the error using a logging framework like Serilog or NLog
                    /*logger.Error(ex, "An error occurred while uploading the resume");

                    // Return an appropriate error message to the user
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while uploading the resume. Please try again later.");*/
                }

            }   

            return new SimpleResume();
        }


        /// <summary>
        /// 此时客户，对第一次上传的文件进行了修改，然后，将其返回
        /// 然后 ，后端返回，是否添加成功
        /// </summary>
        /// <param name="first"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SecondModifyResume")]
        public SecondModifyResultModel SecondModifyResume(SimpleResume simpleResume)
        {
            //将该simpleResume上传数据库
            //返回对应的SecondModifyResultModel
            
            bool status = _applicantService.UpdateApplicant(simpleResume);
            var result = new SecondModifyResultModel();
            if (status)
            {
                result.Code = 20018;
            }
            return result;

        }

    }
}
