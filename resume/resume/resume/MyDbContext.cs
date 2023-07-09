using Microsoft.EntityFrameworkCore;
using resume.Models; // 替换为你的数据模型类的命名空间

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
    public DbSet<WorkExperience> WorkExperiences { get; set; }
    public DbSet<Award> Awards { get; set; }
    public DbSet<SkillCertificate> SkillCertificates { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<EducationBackground> EducationBackgrounds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

        modelBuilder.Entity<User>()
            .HasOne(u => u.Company)
            .WithMany(c => c.Users)
            .HasForeignKey(u => u.CompanyID);

        modelBuilder.Entity<User>()
        .HasIndex(u => u.Account)
        .IsUnique();
    }
}
