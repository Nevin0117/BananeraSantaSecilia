using Microsoft.EntityFrameworkCore;
using SantaSecilia.Domain.Entities;

namespace SantaSecilia.Infrastructure.Data;

/// <summary>
/// SQLite database file is stored at:
///   FileSystem.AppDataDirectory + "/santa_secilia.db3"
///   e.g. Android: /data/data/com.companyname.santasecilia/files/santa_secilia.db3
///        iOS:     ~/Library/Application Support/santa_secilia.db3
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Worker> Workers => Set<Worker>();
    public DbSet<Lot> Lots => Set<Lot>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<WeeklyClosing> WeeklyClosings => Set<WeeklyClosing>();
    public DbSet<DailyRecord> DailyRecords => Set<DailyRecord>();
    public DbSet<RecordVoiding> RecordVoidings => Set<RecordVoiding>();
    public DbSet<WeeklyPayStub> WeeklyPayStubs => Set<WeeklyPayStub>();
    public DbSet<PayStubLine> PayStubLines => Set<PayStubLine>();
    public DbSet<EntityType> EntityTypes => Set<EntityType>();
    public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        // users
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

        // workers
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

        // lots
        b.Entity<Lot>(e =>
        {
            e.ToTable("lots");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Code).HasColumnName("code").IsRequired();
            e.Property(x => x.IsActive).HasColumnName("is_active").IsRequired().HasDefaultValue(true);
            e.Property(x => x.DeactivatedAt).HasColumnName("deactivated_at");  // fix #4
            e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            e.HasIndex(x => x.Code).IsUnique();
        });

        // activities
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

        // weekly_closings
        b.Entity<WeeklyClosing>(e =>
        {
            e.ToTable("weekly_closings", t =>
                t.HasCheckConstraint("ck_weekly_closings_status", "status IN ('CLOSED', 'VOIDED')"));
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.WeekStart).HasColumnName("week_start").IsRequired();
            e.Property(x => x.WeekEnd).HasColumnName("week_end").IsRequired();
            e.Property(x => x.Status).HasColumnName("status").IsRequired();
            e.Property(x => x.SsRateSnapshot).HasColumnName("ss_rate_snapshot").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.SeRateSnapshot).HasColumnName("se_rate_snapshot").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.UnionDuesSnapshot).HasColumnName("union_dues_snapshot").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id").IsRequired();
            e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            e.Property(x => x.Notes).HasColumnName("notes");
            e.HasIndex(x => new { x.WeekStart, x.WeekEnd }).IsUnique();
            e.HasOne(x => x.CreatedByUser).WithMany()
                .HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
        });

        // daily_records
        b.Entity<DailyRecord>(e =>
        {
            e.ToTable("daily_records");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.WorkerId).HasColumnName("worker_id").IsRequired();
            e.Property(x => x.WorkDate).HasColumnName("work_date").IsRequired();
            e.Property(x => x.ActivityId).HasColumnName("activity_id").IsRequired();
            e.Property(x => x.LotId).HasColumnName("lot_id").IsRequired();
            e.Property(x => x.Hours).HasColumnName("hours").IsRequired();
            e.Property(x => x.RateSnapshot).HasColumnName("rate_snapshot").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.SubtotalSnapshot).HasColumnName("subtotal_snapshot").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id").IsRequired();
            e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            e.Property(x => x.UpdatedByUserId).HasColumnName("updated_by_user_id");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            e.Property(x => x.IsLocked).HasColumnName("is_locked").IsRequired().HasDefaultValue(false);
            e.Property(x => x.WeeklyClosingId).HasColumnName("weekly_closing_id");
            e.Property(x => x.AllowDuplicate).HasColumnName("allow_duplicate").IsRequired().HasDefaultValue(false);
            e.HasIndex(x => new { x.WorkerId, x.WorkDate });
            e.HasIndex(x => new { x.WorkerId, x.WorkDate, x.ActivityId, x.LotId });
            e.HasOne(x => x.Worker).WithMany()
                .HasForeignKey(x => x.WorkerId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Activity).WithMany()
                .HasForeignKey(x => x.ActivityId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Lot).WithMany()
                .HasForeignKey(x => x.LotId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<User>().WithMany()
                .HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<User>().WithMany()
                .HasForeignKey(x => x.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<WeeklyClosing>().WithMany()
                .HasForeignKey(x => x.WeeklyClosingId).OnDelete(DeleteBehavior.SetNull);
        });

        // record_voidings
        b.Entity<RecordVoiding>(e =>
        {
            e.ToTable("record_voidings");
            e.HasKey(x => x.RecordId);
            e.Property(x => x.RecordId).HasColumnName("record_id").ValueGeneratedNever();
            e.Property(x => x.VoidedByUserId).HasColumnName("voided_by_user_id").IsRequired();
            e.Property(x => x.VoidReason).HasColumnName("void_reason");
            e.Property(x => x.VoidedAt).HasColumnName("voided_at").IsRequired();
            e.HasOne(x => x.Record).WithOne(r => r.Voiding)
                .HasForeignKey<RecordVoiding>(x => x.RecordId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.VoidedByUser).WithMany()
                .HasForeignKey(x => x.VoidedByUserId).OnDelete(DeleteBehavior.Restrict);
        });

        // weekly_pay_stubs
        b.Entity<WeeklyPayStub>(e =>
        {
            e.ToTable("weekly_pay_stubs");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.WeeklyClosingId).HasColumnName("weekly_closing_id").IsRequired();
            e.Property(x => x.WorkerId).HasColumnName("worker_id").IsRequired();
            e.Property(x => x.TotalHours).HasColumnName("total_hours").IsRequired();
            e.Property(x => x.GrossEarnings).HasColumnName("gross_earnings").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.SsDeduction).HasColumnName("ss_deduction").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.SeDeduction).HasColumnName("se_deduction").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.UnionDues).HasColumnName("union_dues").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.TotalDeductions).HasColumnName("total_deductions").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.NetPay).HasColumnName("net_pay").IsRequired().HasColumnType("TEXT");
            e.Property(x => x.GeneratedAt).HasColumnName("generated_at").IsRequired();
            e.Property(x => x.PdfPath).HasColumnName("pdf_path");
            e.HasIndex(x => new { x.WeeklyClosingId, x.WorkerId }).IsUnique();
            e.HasOne(x => x.WeeklyClosing).WithMany()
                .HasForeignKey(x => x.WeeklyClosingId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Worker).WithMany()
                .HasForeignKey(x => x.WorkerId).OnDelete(DeleteBehavior.Restrict);
        });

        // pay_stub_lines
        b.Entity<PayStubLine>(e =>
        {
            e.ToTable("pay_stub_lines");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.PayStubId).HasColumnName("pay_stub_id").IsRequired();
            e.Property(x => x.DailyRecordId).HasColumnName("daily_record_id").IsRequired();
            e.Property(x => x.Amount).HasColumnName("amount").IsRequired().HasColumnType("TEXT");
            e.HasOne(x => x.PayStub).WithMany(s => s.Lines)
                .HasForeignKey(x => x.PayStubId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.DailyRecord).WithMany()
                .HasForeignKey(x => x.DailyRecordId).OnDelete(DeleteBehavior.Restrict);
        });

        // entity_types
        b.Entity<EntityType>(e =>
        {
            e.ToTable("entity_types");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Name).HasColumnName("name").IsRequired();
            e.HasIndex(x => x.Name).IsUnique();
        });

        // audit_events
        b.Entity<AuditEvent>(e =>
        {
            e.ToTable("audit_events");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
            e.Property(x => x.EntityTypeId).HasColumnName("entity_type_id").IsRequired();
            e.Property(x => x.EntityId).HasColumnName("entity_id").IsRequired();
            e.Property(x => x.Action).HasColumnName("action").IsRequired();
            e.Property(x => x.Details).HasColumnName("details");
            e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            e.HasOne(x => x.User).WithMany()
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.EntityType).WithMany(t => t.AuditEvents)
                .HasForeignKey(x => x.EntityTypeId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
