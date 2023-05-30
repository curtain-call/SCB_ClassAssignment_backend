using Microsoft.EntityFrameworkCore;
using Resume.Class; // 替换为你的数据模型类的命名空间
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
            .WithOne(a => a.Profile)
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
    }
}
