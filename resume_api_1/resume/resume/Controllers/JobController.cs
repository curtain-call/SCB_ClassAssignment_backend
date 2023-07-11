using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using resume.Others;

using resume.ResultModels;
using resume.WebSentModel;
using resume.Others;

namespace resume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {

        /// <summary>
        /// 上传岗位，包括岗位名称，岗位详情，设置选项，最低工作年限，最低学历等。
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        [HttpPost("uploadJobs")]
        public JobIdResultClass UploadJob(JobInfoSentModel jobInfo) {
            //将该Jobinfo存入数据库
            //并返回新的岗位ID

            return new JobIdResultClass();
        
        }

        /// <summary>
        /// 人岗匹配 ：获取某个岗位的人岗匹配结果，包括简历得分排序和人岗匹配原因。
        /// </summary>
        /// <param name="allId"></param>
        /// <returns></returns>
        [HttpPost("Match")]
        public JobMatchResultModelClass JobMatchResume(JobIdSentClass allId)
        {
            int userId=allId.UserId;
            int jobId = allId.JobId;

            //根据userID以及jobID 查数据库 该公司该岗位对应的所有简历
            return new JobMatchResultModelClass();
        }

        /// <summary>
        /// 返回该公司的所有的岗位的id以及信息
        /// </summary>
        /// <param name="webSentUserId"></param>
        /// <returns></returns>
        [HttpPost("allJobName")]
        public AllJobInfoResultClass forAllResumeName(WebSentUserId webSentUserId)
        {
            int userId = webSentUserId.Id;
            //返回该公司的所上传的所有简历的名字以及简历ID
            return new AllJobInfoResultClass();
        }

    }
}
