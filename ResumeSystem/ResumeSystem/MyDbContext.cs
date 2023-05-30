using Microsoft.EntityFrameworkCore;
using ResumeSystem.Class; // 替换为你的数据模型类的命名空间
using ResumeSystem.Models;
using ResumeSystem.ResultModels;
using ResumeSystem.WebSentModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<Applicant> Applicants { get; set; }
    public DbSet<ApplicantProfile> ApplicantProfiles { get; set; }
    public DbSet<JobPosition> JobPositions { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Resume> Resumes { get; set; }

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

        modelBuilder.Entity<Company>()
            .HasMany(c => c.JobPositions)
            .WithOne(jp => jp.Company)
            .HasForeignKey(jp => jp.CompanyID);

        modelBuilder.Entity<Company>()
            .HasMany(c => c.Resumes)
            .WithOne(r => r.Company)
            .HasForeignKey(r => r.CompanyID);
    }

    public LoginModelClass IsLogin(string account, string password)
    {
        var company = Companies.FirstOrDefault(c => c.Account == account && c.Password == password);

        var model = new LoginModelClass();
        model.IsLogin = company != null;
        return model;
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
                                 OverallScore = r.Applicant.ApplicantProfile.OverallScore
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
            HighestEducation = resume.Applicant.EducationLevel,
            WorkExperience = resume.Applicant.WorkExperience,
            OverallScore = resume.Applicant.ApplicantProfile.OverallScore,
            Label = resume.Applicant.ApplicantProfile.TalentTraits
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

}
