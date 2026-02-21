using Microsoft.EntityFrameworkCore;
using SantaSecilia.Domain.Entities;

namespace SantaSecilia.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Worker> Workers => Set<Worker>();
    public DbSet<Lot> Lots => Set<Lot>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<DailyRecord> DailyRecords => Set<DailyRecord>();
    public DbSet<DailyRecordLine> DailyRecordLines => Set<DailyRecordLine>();
    public DbSet<WeeklyPayStub> WeeklyPayStubs => Set<WeeklyPayStub>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Username).HasColumnName("username").IsRequired();
            e.Property(x => x.PasswordHash).HasColumnName("password_hash").IsRequired();
            e.Property(x => x.FullName).HasColumnName("full_name").IsRequired();
            e.Property(x => x.IsActive).HasColumnName("is_active").IsRequired().HasDefaultValue(true);
            e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            e.HasIndex(x => x.Username).IsUnique();
        });

        b.Entity<Worker>(e =>
        {
            e.ToTable("workers");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Code).HasColumnName("code").IsRequired();
            e.Property(x => x.FullName).HasColumnName("full_name").IsRequired();
            e.Property(x => x.IdNumber).HasColumnName("id_number").IsRequired();
            e.Property(x => x.IsActive).HasColumnName("is_active").IsRequired().HasDefaultValue(true);
            e.Property(x => x.DeactivatedAt).HasColumnName("deactivated_at");
            e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            e.HasIndex(x => x.Code).IsUnique();
            e.HasIndex(x => x.IdNumber).IsUnique();
        });

        b.Entity<Lot>(e =>
        {
            e.ToTable("lots");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Code).HasColumnName("code").IsRequired();
            e.Property(x => x.IsActive).HasColumnName("is_active").IsRequired().HasDefaultValue(true);
            e.Property(x => x.DeactivatedAt).HasColumnName("deactivated_at");
            e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            e.HasIndex(x => x.Code).IsUnique();
        });

        b.Entity<Activity>(e =>
        {
            e.ToTable("activities");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Name).HasColumnName("name").IsRequired();
            e.Property(x => x.IsActive).HasColumnName("is_active").IsRequired().HasDefaultValue(true);
            e.Property(x => x.DeactivatedAt).HasColumnName("deactivated_at");
            e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            e.Property(x => x.HourlyRate).HasColumnName("hourly_rate").IsRequired().HasColumnType("TEXT");
            e.HasIndex(x => x.Name).IsUnique();
        });

        b.Entity<DailyRecord>(e =>
        {
            e.ToTable("daily_records");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.WorkerId).HasColumnName("worker_id").IsRequired();
            e.Property(x => x.WorkDate).HasColumnName("work_date").IsRequired();
            e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            e.HasIndex(x => new { x.WorkerId, x.WorkDate });
            e.HasOne(x => x.Worker).WithMany()
                .HasForeignKey(x => x.WorkerId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<DailyRecordLine>(e =>
        {
            e.ToTable("daily_record_lines");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.DailyRecordId).HasColumnName("daily_record_id").IsRequired();
            e.Property(x => x.ActivityId).HasColumnName("activity_id").IsRequired();
            e.Property(x => x.LotId).HasColumnName("lot_id").IsRequired();
            e.Property(x => x.Hours).HasColumnName("hours").IsRequired();
            e.Property(x => x.RateSnapshot).HasColumnName("rate_snapshot").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.SubtotalSnapshot).HasColumnName("subtotal_snapshot").IsRequired().HasColumnType("TEXT");
            e.HasIndex(x => new { x.DailyRecordId, x.ActivityId, x.LotId });
            e.HasOne(x => x.DailyRecord).WithMany(d => d.Lines)
                .HasForeignKey(x => x.DailyRecordId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Activity).WithMany()
                .HasForeignKey(x => x.ActivityId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Lot).WithMany()
                .HasForeignKey(x => x.LotId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<WeeklyPayStub>(e =>
        {
            e.ToTable("weekly_pay_stubs");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.WorkerId).HasColumnName("worker_id").IsRequired();
            e.Property(x => x.WeekStart).HasColumnName("week_start").IsRequired();
            e.Property(x => x.WeekEnd).HasColumnName("week_end").IsRequired();
            e.Property(x => x.TotalHours).HasColumnName("total_hours").IsRequired();
            e.Property(x => x.GrossEarnings).HasColumnName("gross_earnings").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.SsDeduction).HasColumnName("ss_deduction").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.SeDeduction).HasColumnName("se_deduction").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.UnionDues).HasColumnName("union_dues").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.TotalDeductions).HasColumnName("total_deductions").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.NetPay).HasColumnName("net_pay").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.GeneratedAt).HasColumnName("generated_at").IsRequired();
            e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            e.Property(x => x.PdfPath).HasColumnName("pdf_path");
            e.HasIndex(x => new { x.WeekStart, x.WeekEnd, x.WorkerId }).IsUnique();
            e.HasOne(x => x.Worker).WithMany()
                .HasForeignKey(x => x.WorkerId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
