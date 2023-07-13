using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using resume.Models;
using resume.Others;
using resume.ResultModels;
using resume.WebSentModel;
using System.ComponentModel.Design;
//using static Google.Protobuf.Collections.MapField<TKey, TValue>;

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
        public async Task<Applicant> CreateApplicantFromDictionary(CombineDictionary data)
        {
            Dictionary<string, object> basicData = data.BasicResult;
            Dictionary<string, object> matchData = data.JobMatchResult;
            var applicant = new Applicant();
            List<SkillCertificate> skillCertificates = new List<SkillCertificate>();
            List<Award> awards = new List<Award>();
            List<WorkExperience> workExperiences = new List<WorkExperience>();
            List<EducationBackground> educationBackground = new List<EducationBackground>();
            var applicantProfile = new ApplicantProfile
            {
                //TalentTraits = data.ContainsKey("技能") && data["技能"] != null ? data["技能"].ToString() : null,
                MatchingReason = basicData.ContainsKey("人岗匹配的理由") && basicData["人岗匹配的理由"] != null ? basicData["人岗匹配的理由"].ToString() : null,
                MatchingScore = basicData.ContainsKey("人岗匹配程度分数") && basicData["人岗匹配程度分数"] != null ? Convert.ToInt32(basicData["人岗匹配程度分数"]) : 0,
                WorkStability = basicData.ContainsKey("工作稳定性的程度") && basicData["工作稳定性的程度"] != null ? basicData["工作稳定性的程度"].ToString() : null,
                StabilityReason = basicData.ContainsKey("给出此工作稳定性判断的原因") && basicData["给出此工作稳定性判断的原因"] != null ? basicData["给出此工作稳定性判断的原因"].ToString() : null,
            };
            foreach (var pair in basicData)
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
                    case "求职意向岗位":
                        applicant.JobIntention = pair.Value != null ? pair.Value.ToString() : null;
                        break;
                    case "自我评价":
                        applicant.SelfEvaluation = pair.Value != null ? pair.Value.ToString() : null;
                        break;
                    case "最高学历":
                        applicant.HighestEducation = pair.Value != null ? pair.Value.ToString() : null;
                        break;
                    case "最高学历学校等级":
                        applicant.GraduatedFromLevel = pair.Value != null ? pair.Value.ToString() : null;
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
                            workExperiences = JsonConvert.DeserializeObject<List<WorkExperience>>(pair.Value.ToString());
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
                    
                    case "个人特性":
                        {
                            var characteristicsData = (Dictionary<string, object>)pair.Value;
                            applicantProfile.PersonalCharacteristics = ExtractPersonalCharacteristics(characteristicsData);
                            break;
                        }
                    case "技能和经验":
                        {
                            var skillsData = (Dictionary<string, object>)pair.Value;
                            applicantProfile.SkillsAndExperiences = ExtractSkillsAndExperiences(skillsData);
                            break;
                        }
                    case "成就和亮点":
                        {
                            var achievementsData = (Dictionary<string, object>)pair.Value;
                            applicantProfile.AchievementsAndHighlights = ExtractAchievementsAndHighlights(achievementsData);
                            break;
                        }
                }
            }

            foreach (var pair in matchData)
            {
                switch (pair.Key)
                {
                    case "工作总时间":
                        applicant.TotalWorkYears = pair.Value != null ? Convert.ToInt32(pair.Value) : 0;
                        break;
                    // new cases
                    case "产品运营":
                    case "平面设计师":
                    case "财务":
                    case "市场营销":
                    case "项目主管":
                    case "开发工程师":
                    case "文员":
                    case "电商运营":
                    case "人力资源管理":
                    case "风控专员":
                        // ... (all other job titles)
                        {
                            var jobMatchData = (Dictionary<string, object>)pair.Value;
                            var jobMatch = new JobMatch
                            {
                                JobTitle = pair.Key,
                                Score = (int)jobMatchData["人岗匹配程度分数"],
                                Reason = (string)jobMatchData["人岗匹配的理由"],
                            };
                            applicantProfile.JobMatches.Add(jobMatch);
                            break;
                        }
                }
            }
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

            await _dbContext.SaveChangesAsync();

            return applicant;
        }

        public AllSimpleResumeInfoClass ForAllSimpleResumes(int userId)
        {
            // 查找当前用户对应的公司
            var company = _dbContext.Users
                                    .Include(u => u.Company)
                                    .FirstOrDefault(u => u.ID == userId)
                                    ?.Company;

            if (company == null)
            {
                // 如果找不到对应的公司，返回错误信息
                return new AllSimpleResumeInfoClass { SimpleResumes = new List<SimpleResume>(), Code = 60204 };
            }

            // 查询数据库，获取所有属于该用户的简历
            var resumeList = _dbContext.Resumes
                           .Include(r => r.Applicant)
                           .ThenInclude(a => a.ApplicantProfile)
                           .Where(r => r.CompanyID == company.ID)
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
                MatchingScore = _resume.Applicant.ApplicantProfile?.MatchingScore ?? 0
            }).ToList();

            // 创建AllSimpleResumes对象并返回
            var allSimpleResumeInfo = new AllSimpleResumeInfoClass
            {
                SimpleResumes = simpleResumeList,
                Code = 20000
            };

            return allSimpleResumeInfo;
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
            existingApplicant.WorkExperiences = detailedResume.WorkExperience.Select(w => new WorkExperience
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

        private PersonalCharacteristics ExtractPersonalCharacteristics(Dictionary<string, object> data)
        {
            var personalCharacteristics = new PersonalCharacteristics();
            var characteristics = new List<Characteristic>();

            foreach (var pair in data)
            {
                var characteristicData = (Dictionary<string, object>)pair.Value;
                var characteristic = new Characteristic
                {
                    Name = pair.Key,
                    Score = characteristicData["分数"] != null ? Convert.ToInt32(pair.Value) : 0,
                    Reason = characteristicData["原因"].ToString()
                };
                characteristics.Add(characteristic);
            }

            personalCharacteristics.Characteristics = characteristics;

            return personalCharacteristics;
        }

        private SkillsAndExperiences ExtractSkillsAndExperiences(Dictionary<string, object> data)
        {
            var skillsAndExperiences = new SkillsAndExperiences();
            var characteristics = new List<Characteristic>();

            foreach (var pair in data)
            {
                var characteristicData = (Dictionary<string, object>)pair.Value;
                var characteristic = new Characteristic
                {
                    Name = pair.Key,
                    Score = characteristicData["分数"] != null ? Convert.ToInt32(pair.Value) : 0,
                    Reason = characteristicData["原因"].ToString()
                };
                characteristics.Add(characteristic);
            }

            skillsAndExperiences.Characteristics = characteristics;

            return skillsAndExperiences;
        }

        private AchievementsAndHighlights ExtractAchievementsAndHighlights(Dictionary<string, object> data)
        {
            var achievementsAndHighlights = new AchievementsAndHighlights();
            var characteristics = new List<Characteristic>();

            foreach (var pair in data)
            {
                var characteristicData = (Dictionary<string, object>)pair.Value;
                var characteristic = new Characteristic
                {
                    Name = pair.Key,
                    Score = characteristicData["分数"] != null ? Convert.ToInt32(pair.Value) : 0,
                    Reason = characteristicData["原因"].ToString()
                };
                characteristics.Add(characteristic);
            }

            achievementsAndHighlights.Characteristics = characteristics;

            return achievementsAndHighlights;
        }

        public PersonalCharacteristics GetPersonalCharacteristics(int resumeId)
        {
            var applicant = _dbContext.Applicants.Include(a => a.ApplicantProfile)
                                       .ThenInclude(ap => ap.PersonalCharacteristics)
                                       .FirstOrDefault(a => a.ID == resumeId);
            if (applicant == null || applicant.ApplicantProfile == null)
            {
                throw new Exception("Applicant or ApplicantProfile not found");
            }

            return applicant.ApplicantProfile.PersonalCharacteristics;
        }

        public SkillsAndExperiences GetSkillsAndExperiences(int resumeId)
        {
            var applicant = _dbContext.Applicants.Include(a => a.ApplicantProfile)
                                       .ThenInclude(ap => ap.PersonalCharacteristics)
                                       .FirstOrDefault(a => a.ID == resumeId);
            if (applicant == null || applicant.ApplicantProfile == null)
            {
                throw new Exception("Applicant or ApplicantProfile not found");
            }

            return applicant.ApplicantProfile.SkillsAndExperiences;
        }
        public AchievementsAndHighlights GetAchievementsAndHighlights(int resumeId)
        {
            var applicant = _dbContext.Applicants.Include(a => a.ApplicantProfile)
                                       .ThenInclude(ap => ap.PersonalCharacteristics)
                                       .FirstOrDefault(a => a.ID == resumeId);
            if (applicant == null || applicant.ApplicantProfile == null)
            {
                throw new Exception("Applicant or ApplicantProfile not found");
            }

            return applicant.ApplicantProfile.AchievementsAndHighlights;
        }

        public JobMatchScoresInfoForGraphClass JobMatchScoresInfoForGraph(int resumeId) 
        {
            var applicantProfile = _dbContext.ApplicantProfiles.Include(ap => ap.JobMatches)
                                                      .FirstOrDefault(ap => ap.ApplicantID == resumeId);

            if (applicantProfile == null)
            {
                throw new Exception("ApplicantProfile not found");
            }

            var jobMatchScores = new JobMatchScores();

            foreach (var jobMatch in applicantProfile.JobMatches)
            {
                if (jobMatch.Score < 60)
                {
                    jobMatchScores.Below60++;
                }
                else if (jobMatch.Score < 70)
                {
                    jobMatchScores.Range60_70++;
                }
                else if (jobMatch.Score < 80)
                {
                    jobMatchScores.Range70_80++;
                }
                else if (jobMatch.Score < 90)
                {
                    jobMatchScores.Range80_90++;
                }
                else
                {
                    jobMatchScores.Range90_100++;
                }
            }

            return new JobMatchScoresInfoForGraphClass { JobMatchScores = jobMatchScores };
        }
    }
}