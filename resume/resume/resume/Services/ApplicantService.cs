using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using resume.Models;
using resume.Others;
using resume.ResultModels;

namespace resume.Services
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
                        applicant.Name = pair.Value != null ? pair.Value.ToString() : null;
                        break;
                    case "个人邮箱":
                        applicant.Email = pair.Value != null ? pair.Value.ToString() : null;
                        break;
                    case "手机号":
                        applicant.PhoneNumber = pair.Value != null ? pair.Value.ToString() : null;
                        break;
                    case "年龄":
                        applicant.Age = pair.Value != null ? Convert.ToInt32(pair.Value) : 0;
                        break;
                    case "性别":
                        applicant.Gender = pair.Value != null ? pair.Value.ToString() : null;
                        break;
                        break;
                    case "求职意向岗位":
                        applicant.JobIntention = pair.Value != null ? pair.Value.ToString() : null;
                        break;
                    case "自我评价":
                        applicant.SelfEvaluation = pair.Value != null ? pair.Value.ToString() : null;
                        break;
                    case "最高学历":
                        applicant.HighestEducation = pair.Value != null ? pair.Value.ToString() : null;
                        break;
                    case "毕业院校":
                        applicant.GraduatedFrom = pair.Value != null ? pair.Value.ToString() : null;
                        break;
                    case "专业":
                        applicant.Major = pair.Value != null ? pair.Value.ToString() : null;
                        break;
                    case "技能证书":
                        // assuming SkillCertificates field is a comma-separated string
                        if (pair.Value != null)
                        {
                            string[] skills = pair.Value.ToString().Split(',');
                            foreach (var skill in skills)
                            {
                                // Remove unnecessary characters like "{", "}", "[", "]", and '"'
                                string cleanedSkill = skill.Trim().Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "").Replace("\"", "").Replace("\r", "").Replace("\n", "");
                                skillCertificates.Add(new SkillCertificate { SkillName = cleanedSkill, ApplicantID = applicant.ID });
                            }
                        }
                        break;
                    case "获奖荣誉":
                        if (pair.Value != null) // check if pair.Value is not null
                        {
                            // assuming Awards field is a comma-separated string
                            string[] awardList = pair.Value.ToString().Split(',');
                            foreach (var award in awardList)
                            {
                                // Remove unnecessary characters like "{", "}", "[", "]", and '"'
                                string cleanedAward = award.Trim().Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "").Replace("\"", "").Replace("\r", "").Replace("\n", "");
                                awards.Add(new Award { AwardName = cleanedAward, ApplicantID = applicant.ID });
                            }
                        }
                        break;
                    case "工作经历":
                        if (pair.Value != null) // check if pair.Value is not null
                        {
                            // assuming WorkExperiences field is a JSON array of WorkExperiences objects
                            workExperiences = JsonConvert.DeserializeObject<List<WorkExperiences>>(pair.Value.ToString());
                            foreach (var exp in workExperiences)
                            {
                                exp.ApplicantID = applicant.ID;
                            }
                        }

                        break;
                    case "教育背景":
                        if (pair.Value != null)
                        {
                            educationBackground = JsonConvert.DeserializeObject<List<EducationBackground>>(pair.Value.ToString());
                            foreach (var exp in workExperiences)
                            {
                                exp.ApplicantID = applicant.ID;
                            }
                        }
                        break;
                    case "工作总时间":
                        applicant.TotalWorkYears = pair.Value != null ? pair.Value.ToString() : null;
                        break;
                }
            }

            var applicantProfile = new ApplicantProfile
            {
                //TalentTraits = data.ContainsKey("技能") && data["技能"] != null ? data["技能"].ToString() : null,
                MatchingReason = data.ContainsKey("人岗匹配的理由") && data["人岗匹配的理由"] != null ? data["人岗匹配的理由"].ToString() : null,
                MatchingScore = data.ContainsKey("人岗匹配程度分数") && data["人岗匹配程度分数"] != null ? Convert.ToInt32(data["人岗匹配程度分数"]) : 0,
                WorkStability = data.ContainsKey("工作稳定性的程度") && data["工作稳定性的程度"] != null ? data["工作稳定性的程度"].ToString() : null,
                StabilityReason = data.ContainsKey("给出此工作稳定性判断的原因") && data["给出此工作稳定性判断的原因"] != null ? data["给出此工作稳定性判断的原因"].ToString() : null,
            };

            applicant.ApplicantProfile = applicantProfile;

            _dbContext.Applicants.Add(applicant);
            await _dbContext.SaveChangesAsync();

            // 设置每个 SkillCertificate 对象的 ApplicantID，并添加到数据库
            foreach (var skill in skillCertificates)
            {
                skill.ApplicantID = applicant.ID;
            }
            _dbContext.SkillCertificates.AddRange(skillCertificates);

            // 设置每个 Award 对象的 ApplicantID，并添加到数据库
            foreach (var award in awards)
            {
                award.ApplicantID = applicant.ID;
            }
            _dbContext.Awards.AddRange(awards);

            // 设置每个 WorkExperiences 对象的 ApplicantID，并添加到数据库
            foreach (var exp in workExperiences)
            {
                exp.ApplicantID = applicant.ID;
            }
            _dbContext.WorkExperiences.AddRange(workExperiences);

            // 设置每个 EducationBackground 对象的 ApplicantID，并添加到数据库
            foreach (var edu in educationBackground)
            {
                edu.ApplicantID = applicant.ID;
            }
            _dbContext.EducationBackgrounds.AddRange(educationBackground);

            // 最后保存这些修改
            await _dbContext.SaveChangesAsync();

            return applicant;
        }

        public AllSimpleResumes ForAllSimpleResumes(int userId)
        {
            // 查询数据库，获取所有属于该用户的简历
            var resumeList = _dbContext.Resumes
                           .Include(r => r.Applicant)
                           .ThenInclude(a => a.ApplicantProfile)
                           .Where(r => r.CompanyID == userId)
                           .ToList();

            // 将每个Applicant对象转换为SimpleResume对象
            var simpleResumeList = resumeList.Select(_resume => new SimpleResume
            {
                Rid = _resume.Applicant.ID,
                Name = _resume.Applicant.Name,
                Age = _resume.Applicant.Age,
                HighestEducation = _resume.Applicant.HighestEducation,
                PhoneNumber = _resume.Applicant.PhoneNumber,
                JobIntention = _resume.Applicant.JobIntention,
                Gender = _resume.Applicant.Gender,
                MatChingScore = _resume.Applicant.ApplicantProfile?.MatchingScore ?? 0
            }).ToList();

            // 创建AllSimpleResumes对象并返回
            var allSimpleResumes = new AllSimpleResumes
            {
                SimpleResumes = simpleResumeList
            };

            return allSimpleResumes;
        }

        public Boolean UpdateApplicant(DetailedResume detailedResume)
        {
            var resume = _dbContext.Resumes.FirstOrDefault(r => r.ID == detailedResume.Id);

            if (resume == null) { return false; }

            var existingApplicant = _dbContext.Applicants
                .Include(a => a.ApplicantProfile)
                .Include(a => a.WorkExperiences)
                .Include(a => a.SkillCertificates)
                .Include(a => a.EducationBackgrounds)
                .Include(a => a.Awards)
                .FirstOrDefault(applicant => applicant.ID == resume.ApplicantID);
            if (existingApplicant == null) { return false; }
            existingApplicant.Name = detailedResume.Name;
            existingApplicant.Email = detailedResume.Email;
            existingApplicant.PhoneNumber = detailedResume.PhoneNumber;
            existingApplicant.Age = detailedResume.Age;
            existingApplicant.Gender = detailedResume.Gender;
            existingApplicant.JobIntention = detailedResume.JobIntention;
            existingApplicant.SelfEvaluation = detailedResume.SelfEvaluation;
            existingApplicant.HighestEducation = detailedResume.HighestEducation;
            existingApplicant.ApplicantProfile.WorkStability = detailedResume.WorkStability;
            existingApplicant.ApplicantProfile.StabilityReason = detailedResume.WorkStabilityReason;
            existingApplicant.ApplicantProfile.MatchingScore = detailedResume.MatchingScore;
            existingApplicant.ApplicantProfile.MatchingReason = detailedResume.MatchingReason;

            // Handle WorkExperiences
            _dbContext.WorkExperiences.RemoveRange(existingApplicant.WorkExperiences); // Remove existing
            existingApplicant.WorkExperiences = detailedResume.WorkExperience.Select(w => new WorkExperiences
            {
                ApplicantID = existingApplicant.ID,
                CompanyName = w.CompanyName,
                Position = w.Position,
                Time = w.Time,
                Task = w.Task
            }).ToList();  // Add new

            // Handle SkillCertificates
            _dbContext.SkillCertificates.RemoveRange(existingApplicant.SkillCertificates); // Remove existing
            existingApplicant.SkillCertificates = detailedResume.SkillCertificate.Select(s => new SkillCertificate
            {
                ApplicantID = existingApplicant.ID,
                SkillName = s.SkillName
            }).ToList();  // Add new

            // Handle Awards
            _dbContext.Awards.RemoveRange(existingApplicant.Awards); // Remove existing
            existingApplicant.Awards = detailedResume.Awards.Select(a => new Award
            {
                ApplicantID = existingApplicant.ID,
                AwardName = a.AwardName
            }).ToList();  // Add new
            _dbContext.SaveChanges();

            if (detailedResume.EducationBackgrounds == null)
            {
                detailedResume.EducationBackgrounds = new List<EducationBackground>();
            }
            _dbContext.EducationBackgrounds.RemoveRange(existingApplicant.EducationBackgrounds); // Remove existing
            existingApplicant.EducationBackgrounds = detailedResume.EducationBackgrounds.Select(ed => new EducationBackground
            {
                ApplicantID = existingApplicant.ID,
                Time = ed.Time,
                School = ed.School,
                Major = ed.Major
            }).ToList();  // Add new
            _dbContext.SaveChanges();
            return true;
        }

    }
}