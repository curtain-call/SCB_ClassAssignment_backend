using Microsoft.AspNetCore.Mvc;
using ResumeSystem.ResultModel;
using ResumeSystem.WebSentModel;
using ResumeSystem.Services;

namespace ResumeSystem.Controllers
{
    /// <summary>
    /// 这个类，主要赋值的是，与前端的岗位信息进行交互
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly ApplicantService _applicantService;

        public JobController(ApplicantService applicantService)
        {
            _applicantService = applicantService;
        }


        /// <summary>
        /// 当点击其他职位，非默认的职位时，返回岗位要求+岗位简历排序【ID(不显示)+人名+综合分】
        /// 当由于能力有限，不知道怎么一次传两个数组，因而只能分为两次post请求
        /// 第一次传岗位要求
        /// 第二次传岗位简历综合排序
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="JobId"></param>
        /// <returns></returns>

/*        [HttpPost]
        [Route("ChangeJob1")]
        public IActionResult ChangeJob1(int companyId, int jobId)
        {
            var jobRequirement = _context.GetJobRequirement(jobId);
            if (jobRequirement == null || jobRequirement.CompanyID != companyId)
            {
                return NotFound(new { message = "Job not found or does not belong to the company." });
            }

            var sortedResumes = _context.GetSortedResumes(jobId);

            var result = new
            {
                JobRequirement = jobRequirement.Description,
                SortedResumes = sortedResumes.Select(r => new
                {
                    r.ID,
                    ApplicantName = r.Applicant.Name,
                    MatchingScore = r.ApplicantProfile.MatchingScore
                })
            };

            return Ok(result);
        }*/

    }
}

