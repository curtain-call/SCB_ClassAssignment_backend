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
    public DbSet<WorkExperiences> WorkExperiences { get; set; }
    public DbSet<Award> Awards { get; set; }
    public DbSet<SkillCertificate> SkillCertificates { get; set; }
    public DbSet<EducationBackground> EducationBackgrounds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Applicant>()
            .HasKey(a => a.ID); // 设置主键

        modelBuilder.Entity<ApplicantProfile>()
            .HasKey(ap => ap.ID); // 设置主键

        modelBuilder.Entity<JobPosition>()
            .HasKey(jp => jp.ID); // 设置主键

        modelBuilder.Entity<Company>()
            .HasKey(c => c.ID); // 设置主键

        modelBuilder.Entity<Resume>()
            .HasKey(r => r.ID); // 设置主键

        modelBuilder.Entity<WorkExperiences>()
            .HasKey(w => w.ID);

        modelBuilder.Entity<Award>()
            .HasKey(a => a.ID);

        modelBuilder.Entity<SkillCertificate>()
            .HasKey(sk => sk.ID);

        modelBuilder.Entity<EducationBackground>()
            .HasKey(ed => ed.ID);

        modelBuilder.Entity<Resume>()
            .HasOne(r => r.Applicant) // 设置一对一关系
            .WithOne(a => a.Resume)
            .HasForeignKey<Resume>(r => r.ApplicantID);

        modelBuilder.Entity<Resume>()
            .HasOne(r => r.JobPosition) // 设置一对一关系
            .WithMany(jp => jp.Resumes)
            .HasForeignKey(r => r.JobPositionID);

        modelBuilder.Entity<Applicant>()
            .HasOne(a => a.ApplicantProfile)
            .WithOne(ap => ap.Applicant)
            .HasForeignKey<ApplicantProfile>(ap => ap.ApplicantID);

        modelBuilder.Entity<Applicant>()
            .HasMany(a => a.Awards)
            .WithOne()
            .HasForeignKey(aw => aw.ApplicantID);

        modelBuilder.Entity<Applicant>()
            .HasMany(a => a.WorkExperiences)
            .WithOne()
            .HasForeignKey(wo => wo.ApplicantID);

        modelBuilder.Entity<Applicant>()
            .HasMany(a => a.SkillCertificates)
            .WithOne()
            .HasForeignKey(sk => sk.ApplicantID);

        modelBuilder.Entity<Applicant>()
            .HasMany(a => a.EducationBackgrounds)
            .WithOne()
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
            //TalentTraits = resume.Applicant.ApplicantProfile.TalentTraits
        };

        return singleResumeModelClass;
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
            existingResume.Applicant.WorkExperiences = first.WorkExperiences;
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
                      .OrderByDescending(r => r.Applicant.ApplicantProfile.MatchingScore)
                      .ToList();
    }
}
