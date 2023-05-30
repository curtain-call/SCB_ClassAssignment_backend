using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResumeSystem.ResultModels;
using ResumeSystem.Class;
using Microsoft.EntityFrameworkCore;

namespace ResumeSystem.Controllers
{
    /// <summary>
    /// 这个类，主要赋值的是，与前端的岗位信息进行交互
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly MyDbContext _context;

        public JobController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("MainToJob/1")]
        public IActionResult MainToJob(int companyId)
        {
            var company = _context.Companies.FirstOrDefault(c => c.ID == companyId);
            if (company == null)
            {
                return BadRequest(new { Error = "Company not found" });
            }

            var jobNames = _context.GetJobNames(companyId);
            var firstJobRequirement = _context.GetFirstJobRequirement(companyId);

            // 将结果返回给前端
            return Ok(new { JobNames = jobNames, FirstJobRequirement = firstJobRequirement });
        }

        /// <summary>
        /// 实现的是，当从主页向job页进行跳转的时候，应该返回
        /// 该公司所有的全部岗位名字+【第一个】岗位要求+岗位简历排序【ID(不显示)+人名+综合分】
        /// 分三个post传送
        /// 第一个传送firstid 第二个传送 第一个岗位的要求 第三个传送岗位简历排序
        /// </summary>
        /// <param name="CompanyId"></param>
        /*[HttpPost]
        [Route("MainToJob/1")]
        public int MainToJob1(int CompanyId)
        {
            //调用接口，返回第一个岗位ID
            //传入公司ID
            return CompanyId;

        }

        /// <summary>
        /// 还是当主页从Main跳转时返回的数据
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="JobId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("MainToJob/2")]
        public IEnumerable<string> MainToJob2(int CompanyId, int JobId)
        {
            //调用接口 查找数据库
            //传入：int CompanyId,int JobId   返回：List<sting> 就是岗位需求的数组

            List<string> result = new List<string>();
            result.Add("adad");
            result.Add($"{CompanyId} {JobId}");
            return result;

        }

        [HttpPost]
        [Route("MainToJob/3")]
        public IEnumerable<Job> MainToJob3(int CompanyId)
        {
            //调用接口 返回该公司id所对应的所有岗位
            //传入：CompanyId 传出：List<job>

            //测试
            Job job1 = new Job();
            job1.JobId = 2;
            job1.JobName = "Test";
            Job job2 = new Job();
            job2.JobName = "Test";
            job2.JobId = 3;
            List<Job> result = new List<Job>();
            result.Add(job1);
            result.Add(job2);
            return result;
        }


        [HttpPost]
        [Route("MainToJob/4")]
        public IEnumerable<Applicant> MainToJob4(int CompanyId, int JobId)
        {
            //调用接口 返回该公司第一个岗位的所有的求职者
            //传入：CompanyId JobId 传出：List<applicantClass>

            //测试

            Applicant Applicant1 = new Applicant();
            //Applicant1.allScore = 1;
            Applicant1.Name = "Test";
            //Applicant1.ApplicantId = CompanyId;
            Applicant Applicant2 = new Applicant();
            //Applicant2.allScore = 2;
            Applicant2.Name = "Test";
            //Applicant2.ApplicantId = CompanyId;
            List<Applicant> result = new List<Applicant>();
            result.Add(Applicant1);
            result.Add(Applicant2);
            return result;

        }*/


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
        [Route("ChangeJob/1")]
        public IEnumerable<string> ChangeJob1(int CompanyId, int JobId){
            //调用接口 传入：int CompanyId, int JobId
            //传出：对应岗位的所有岗位需求

            List<string> result = new List<string>();
            result.Add("adad");
            result.Add("adadfvv");
            return  result;

        }

        [HttpPost]
        [Route("ChangeJob/2")]
        public IEnumerable<Applicant> ChangeJob2(int CompanyId, int JobId)
        {
            //调用接口 传入：int CompanyId, int JobId
            //传出：对应岗位的所有岗位需求

            //测试

            Applicant Applicant1 = new Applicant();
            //Applicant1. = 1;
            Applicant1.Name = "Test";
            // Applicant1.ApplicantId = CompanyId;
            Applicant Applicant2 = new Applicant();
            //Applicant2.allScore = 2;
            Applicant2.Name = "Test";
            //Applicant2.ApplicantId = CompanyId;
            List<Applicant> result = new List<Applicant>();
            result.Add(Applicant1);
            result.Add(Applicant2);
            return result;

        }*/
        [HttpPost]
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
        }

    }
}

