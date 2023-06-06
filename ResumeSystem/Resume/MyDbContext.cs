using Microsoft.EntityFrameworkCore;
using ResumeSystem.Models; // 替换为你的数据模型类的命名空间
using ResumeSystem.ResultModels;
using ResumeSystem.WebSentModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
        this.Database.EnsureCreated();
    }

    public DbSet<Applicant> Applicants { get; set; }
    public DbSet<ApplicantProfile> ApplicantProfiles { get; set; }
    public DbSet<JobPosition> JobPositions { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Resume> Resumes { get; set; }
    public DbSet<WorkExperiences> WorkExperience { get; set; }
    public DbSet<Award> Awards { get; set; }
    public DbSet<SkillCertificate> SkillCertificates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Applicant>()
            .HasKey(a => a.ID); // 设置主键

        modelBuilder.Entity<ApplicantProfile>()
            .HasKey(ap => ap.ID); // 设置主键

        modelBuilder.Entity<ApplicantProfile>()
            .HasOne(ap => ap.Applicant) // 设置一对一关系
            .WithOne(a => a.ApplicantProfile)//?
            .HasForeignKey<ApplicantProfile>(ap => ap.ApplicantID);

        modelBuilder.Entity<JobPosition>()
            .HasKey(jp => jp.ID); // 设置主键

        modelBuilder.Entity<Company>()
            .HasKey(c => c.ID); // 设置主键

        modelBuilder.Entity<Resume>()
            .HasKey(r => r.ID); // 设置主键

        modelBuilder.Entity<WorkExperiences>()
            .HasKey(w => w.Id);

        modelBuilder.Entity<Award>()
            .HasKey(a => a.ID);

        modelBuilder.Entity<Resume>()
            .HasOne(r => r.Applicant) // 设置一对一关系
            .WithOne(a => a.Resume)
            .HasForeignKey<Resume>(r => r.ApplicantID);

        modelBuilder.Entity<Resume>()
            .HasOne(r => r.JobPosition) // 设置一对一关系
            .WithOne(jp => jp.Resume)
            .HasForeignKey<Resume>(r => r.JobPositionID);

        modelBuilder.Entity<Applicant>()
            .HasOne(a => a.ApplicantProfile)
            .WithOne(ap => ap.Applicant)
            .HasForeignKey<ApplicantProfile>(ap => ap.ApplicantID);

        modelBuilder.Entity<Applicant>()
            .HasOne(a => a.Resume)
            .WithOne(r => r.Applicant)
            .HasForeignKey<Resume>(r => r.ApplicantID);

        modelBuilder.Entity<Applicant>()
            .HasMany(a => a.Awards)
            .WithOne(aw => aw.Applicant)
            .HasForeignKey(aw => aw.ApplicantID);

        modelBuilder.Entity<Applicant>()
            .HasMany(a => a.WorkExperiences)
            .WithOne(wo => wo.Applicant)
            .HasForeignKey(wo => wo.ApplicantID);

        modelBuilder.Entity<Applicant>()
            .HasMany(a => a.SkillCertificates)
            .WithOne(sk => sk.Applicant)
            .HasForeignKey(sk => sk.ApplicantID);

        modelBuilder.Entity<Applicant>()
            .HasMany(a => a.EducationBackgrounds)
            .WithOne(ed => ed.Applicant)
            .HasForeignKey(ed => ed.ApplicantID);

        modelBuilder.Entity<Company>()
            .HasMany(c => c.JobPositions)
            .WithOne(jp => jp.Company)
            .HasForeignKey(jp => jp.CompanyID);

        modelBuilder.Entity<Company>()
            .HasMany(c => c.Resumes)
            .WithOne(r => r.Company)
            .HasForeignKey(r => r.CompanyID);
    }

    public RegisterModelClass CreateNewAccount(RegisterSentModels register)
    {
        var company = Companies.FirstOrDefault(c => c.Account == register.Account);
        var model = new RegisterModelClass();

        if (company != null)
        {
            // 账号已经存在
            model.IsSuccess = false;
            model.Msg = "Account already exists.";
        }
        else
        {
            // 添加新的公司
            company = new Company
            {
                Account = register.Account,
                Email = register.Email,
                Password = register.Password
            };
            Companies.Add(company);
            SaveChanges();

            model.IsSuccess = true;
            model.Msg = "Registration successful.";
        }

        return model;
    }
    public IEnumerable<ResumeDTO> SearchByCompanyId(int companyId)
    {
        var resumes = Resumes.Include(r => r.Applicant)
                             .ThenInclude(a => a.ApplicantProfile)
                             .Include(r => r.JobPosition)
                             .Where(r => r.JobPosition.CompanyID == companyId)
                             .Select(r => new ResumeDTO
                             {
                                 ResumeId = r.ID,
                                 ApplicantName = r.Applicant.Name,
                                 JobPosition = r.JobPosition.Title,
                                 MatchingScore = r.Applicant.ApplicantProfile.MatchingScore
                             })
                             .ToList();
        return resumes;
    }
    public SingleResumeModelClass GetResumeById(int resumeId)
    {
        var resume = Resumes.Include(r => r.Applicant)
                            .ThenInclude(a => a.ApplicantProfile)
                            .Include(r => r.JobPosition)
                            .FirstOrDefault(r => r.ID == resumeId);

        if (resume == null)
        {
            return null;
        }

        var singleResumeModelClass = new SingleResumeModelClass
        {
            ResuemId = resume.ID,
            Age = resume.Applicant.Age,
            Name = resume.Applicant.Name,
            Email = resume.Applicant.Email,
            JobPosition = resume.JobPosition.Title,
            Gender = resume.Applicant.Gender,
            HighestEducation = resume.Applicant.HighestEducation,
            MatchingScore = resume.Applicant.ApplicantProfile.MatchingScore,
            TalentTraits = resume.Applicant.ApplicantProfile.TalentTraits
        };

        return singleResumeModelClass;
    }

    public int AddResumePath(string filePath)
    {
        var resume = new Resume
        {
            FilePath = filePath
        };
        Resumes.Add(resume);
        SaveChanges();
        return resume.ID;
    }

    /*public SecondAddResumeModelClass UpdateResume(FirstAddResumeModelClass first)
    {
        // 查找是否已经存在相同的简历
        var existingResume = Resumes
            .Include(r => r.Applicant)
            .Include(r => r.JobPosition)
            .FirstOrDefault(r => r.ID == first.ResumeID);

        // 如果存在，则更新简历内容
        if (existingResume != null)
        {
            existingResume.Applicant.Name = first.Name;
            existingResume.Applicant.Email = first.Email;
            existingResume.JobPosition.Title = first.JobIntention;
            existingResume.Applicant.PhoneNumber = first.PhoneNumber;
            existingResume.Applicant.Age = first.Age;
            existingResume.Applicant.Awards = first.Awards;
            existingResume.Applicant.Gender = first.Gender;
            existingResume.Applicant.WorkExperience = first.WorkExperience;
            existingResume.Applicant.TotalWorkYears = first.TotalWorkYears;
            existingResume.Applicant.SelfEvaluation = first.SelfEvaluation;
            existingResume.Applicant.SkillCertificates = first.SkillCertificate;
            // 其他需要更新的字段...

            SaveChanges();

            return new SecondAddResumeModelClass { AddSuccess = true };
        }
        // 如果不存在，则创建新的简历
        else
        {
            return new SecondAddResumeModelClass { AddSuccess = false, ErrorMessage = "Resume not found." };
        }
    }*/
    //获取公司的所有职位名称和第一个职位的要求。
    public List<string> GetJobNames(int companyId)
    {
        return JobPositions.Where(jp => jp.CompanyID == companyId)
                           .Select(jp => jp.Title)
                           .ToList();
    }

    public string GetFirstJobRequirement(int companyId)
    {
        var firstJob = JobPositions.FirstOrDefault(jp => jp.CompanyID == companyId);
        return firstJob != null ? firstJob.Description : null;
    }

    public JobPosition GetJobRequirement(int jobId)
    {
        return JobPositions.FirstOrDefault(jp => jp.ID == jobId);
    }

    public List<Resume> GetSortedResumes(int jobId)
    {
        return Resumes.Include(r => r.Applicant)
                      .Include(r => r.JobPosition)
                      .Where(r => r.JobPosition.ID == jobId)
                      .OrderByDescending(r => r.ApplicantProfile.MatchingScore)
                      .ToList();
    }
}
