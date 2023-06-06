using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ResumeSystem.Models;
using ResumeSystem.ResultModels;
using ResumeSystem.WebSentModel;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace ResumeSystem.Services
{
    public class ApplicantService
    {
        private readonly MyDbContext _dbContext;

        public ApplicantService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Applicant> GetAllApplicants()
        {
            return _dbContext.Applicants.ToList();
        }

        public Applicant GetApplicantById(int id)
        {
            return _dbContext.Applicants.FirstOrDefault(a => a.ID == id);
        }

        // Add more methods for querying the Applicants table as needed
        public async Task<Applicant> CreateApplicantFromDictionary(Dictionary<string, object> data)
        {
            var applicant = new Applicant();
            List<SkillCertificate> skillCertificates = new List<SkillCertificate>();
            List<Award> awards = new List<Award>();
            List<WorkExperiences> workExperiences = new List<WorkExperiences>();
            List<EducationBackground> educationBackground = new List<EducationBackground>();
            foreach (var pair in data)
            {
                switch (pair.Key)
                {                 
                    case "姓名":
                        applicant.Name = pair.Value.ToString();
                        break;
                    case "个人邮箱":
                        applicant.Email = pair.Value.ToString();
                        break;
                    case "手机号":
                        applicant.PhoneNumber = pair.Value.ToString();
                        break;
                    case "年龄":
                        applicant.Age = (int)pair.Value;
                        break;
                    case "性别":
                        applicant.Gender = pair.Value.ToString();
                        break;
                    case "求职意向岗位":
                        applicant.JobIntention = pair.Value.ToString();
                        break;
                    case "自我评价":
                        applicant.SelfEvaluation = pair.Value.ToString();
                        break;
                    case "最高学历":
                        applicant.HighestEducation = pair.Value.ToString();
                        break;
                    case "毕业院校":
                        applicant.GraduatedFrom = pair.Value.ToString();
                        break;
                    case "专业":
                        applicant.Major = pair.Value.ToString();
                        break;
                    case "技能证书":
                        // assuming SkillCertificates field is a comma-separated string
                        string[] skills = pair.Value.ToString().Split(',');
                        foreach (var skill in skills)
                        {
                            skillCertificates.Add(new SkillCertificate { SkillName = skill, ApplicantID = applicant.ID });
                        }
                        break;
                    case "Awards":
                        // assuming Awards field is a comma-separated string
                        string[] awardList = pair.Value.ToString().Split(',');
                        foreach (var award in awardList)
                        {
                            awards.Add(new Award { AwardName = award, ApplicantID = applicant.ID });
                        }
                        break;
                    case "工作经历":
                        // assuming WorkExperience field is a JSON array of WorkExperience objects
                        workExperiences = JsonConvert.DeserializeObject<List<WorkExperiences>>(pair.Value.ToString());
                        foreach (var exp in workExperiences)
                        {
                            exp.ApplicantID = applicant.ID;
                        }
                        break;
                    case "教育背景":
                        educationBackground = JsonConvert.DeserializeObject<List<EducationBackground>>(pair.Value.ToString());
                        foreach (var exp in workExperiences)
                        {
                            exp.ApplicantID = applicant.ID;
                        }
                        break;
                    case "工作总时间":
                        applicant.TotalWorkYears = int.Parse(pair.Value.ToString());
                        break;
                    default:
                        throw new Exception($"Invalid key in data dictionary: {pair.Key}");
                }
            }

            var applicantProfile = new ApplicantProfile
            {
                TalentTraits = data.ContainsKey("技能") ? data["技能"].ToString() : null,
                MatchingReason = data.ContainsKey("人岗匹配的理由") ? data["人岗匹配的理由"].ToString() : null,
                MatchingScore = data.ContainsKey("人岗匹配程度分数") ? (int)data["人岗匹配程度分数"] : 0,
                WorkStability = data.ContainsKey("工作稳定性的程度") ? data["工作稳定性的程度"].ToString() : null,
                StabilityReason = data.ContainsKey("给出此工作稳定性判断的原因") ? data["给出此工作稳定性判断的原因"].ToString() : null,
            };

            applicant.ApplicantProfile = applicantProfile;

            _dbContext.Applicants.Add(applicant);
            await _dbContext.SaveChangesAsync();

            return applicant;
        }

        public AllSimpleResumes ForAllSimpleResumes(int userId)
        {
            // 查询数据库，获取所有属于该用户的简历
            var applicantList = _dbContext.Applicants.Where(applicant => applicant.ID == userId).ToList();

            // 将每个Applicant对象转换为SimpleResume对象
            var simpleResumeList = applicantList.Select(applicant => new SimpleResume
            {
                Rid = applicant.ID,
                Age = applicant.Age,
                PhoneNumber = applicant.PhoneNumber,
                JobIntention = applicant.JobIntention,
                Gender = applicant.Gender,
                MatChingScore = applicant.ApplicantProfile?.MatchingScore ?? 0 // 这里假设你的Applicant类有一个名为ApplicantProfile的导航属性，如果不是这样，你需要根据实际情况进行调整
            }).ToList();

            // 创建AllSimpleResumes对象并返回
            var allSimpleResumes = new AllSimpleResumes
            {
                SimpleResumes = simpleResumeList
            };

            return allSimpleResumes;
        }

        public Boolean UpdateApplicant(SimpleResume simpleResume)
        {
            var existingApplicant = _dbContext.Applicants.FirstOrDefault(applicant => applicant.ID == simpleResume.Rid);
            if (existingApplicant == null) { return false; }
            existingApplicant.Age = simpleResume.Age;
            existingApplicant.PhoneNumber = simpleResume.PhoneNumber;
            existingApplicant.JobIntention = simpleResume.JobIntention;
            existingApplicant.Gender = simpleResume.Gender;
            return true;
        }

    }
}