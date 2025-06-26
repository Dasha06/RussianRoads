using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace russianRoads.Models;

public partial class Demo33dbContext : DbContext
{
    public Demo33dbContext()
    {
    }

    public Demo33dbContext(DbContextOptions<Demo33dbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cabinet> Cabinets { get; set; }

    public virtual DbSet<CalendarLearning> CalendarLearnings { get; set; }

    public virtual DbSet<CalendarMissedWorker> CalendarMissedWorkers { get; set; }

    public virtual DbSet<CalendarWorkersHoliday> CalendarWorkersHolidays { get; set; }

    public virtual DbSet<Ismain> Ismains { get; set; }

    public virtual DbSet<LearnAndWorker> LearnAndWorkers { get; set; }

    public virtual DbSet<LearnEvent> LearnEvents { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<OrganizationsHierarchy> OrganizationsHierarchies { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Suborganization> Suborganizations { get; set; }

    public virtual DbSet<Subsuborganization> Subsuborganizations { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    public virtual DbSet<Workingcalendar> Workingcalendars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=demo33db;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cabinet>(entity =>
        {
            entity.HasKey(e => e.CabId).HasName("cabinets_pkey");

            entity.ToTable("cabinets", "Voronzov2");

            entity.Property(e => e.CabId)
                .ValueGeneratedNever()
                .HasColumnName("cab_id");
            entity.Property(e => e.CabNumber).HasColumnName("cab_number");
        });

        modelBuilder.Entity<CalendarLearning>(entity =>
        {
            entity.HasKey(e => e.CalenlearnId).HasName("calendar_learning_pkey");

            entity.ToTable("calendar_learning", "Voronzov2");

            entity.Property(e => e.CalenlearnId)
                .ValueGeneratedNever()
                .HasColumnName("calenlearn_id");
            entity.Property(e => e.CalenlearnDateEnd).HasColumnName("calenlearn_date_end");
            entity.Property(e => e.CalenlearnDateStart).HasColumnName("calenlearn_date_start");
            entity.Property(e => e.CalenlearnEventId).HasColumnName("calenlearn_event_id");

            entity.HasOne(d => d.CalenlearnEvent).WithMany(p => p.CalendarLearnings)
                .HasForeignKey(d => d.CalenlearnEventId)
                .HasConstraintName("calendar_learning_calenlearn_event_id_fkey");
        });

        modelBuilder.Entity<CalendarMissedWorker>(entity =>
        {
            entity.HasKey(e => e.CalenmissId).HasName("calendar_missed_workers_pkey");

            entity.ToTable("calendar_missed_workers", "Voronzov2");

            entity.Property(e => e.CalenmissId)
                .ValueGeneratedNever()
                .HasColumnName("calenmiss_id");
            entity.Property(e => e.CalenmissDate).HasColumnName("calenmiss_date");
            entity.Property(e => e.WorkerMissedId).HasColumnName("worker_missed_id");
            entity.Property(e => e.WorkerReplacedId).HasColumnName("worker_replaced_id");

            entity.HasOne(d => d.WorkerMissed).WithMany(p => p.CalendarMissedWorkerWorkerMisseds)
                .HasForeignKey(d => d.WorkerMissedId)
                .HasConstraintName("calendar_missed_workers_worker_missed_id_fkey");

            entity.HasOne(d => d.WorkerReplaced).WithMany(p => p.CalendarMissedWorkerWorkerReplaceds)
                .HasForeignKey(d => d.WorkerReplacedId)
                .HasConstraintName("calendar_missed_workers_worker_replaced_id_fkey");
        });

        modelBuilder.Entity<CalendarWorkersHoliday>(entity =>
        {
            entity.HasKey(e => e.CalenholidayId).HasName("calendar_workers_holidays_pkey");

            entity.ToTable("calendar_workers_holidays", "Voronzov2");

            entity.Property(e => e.CalenholidayId)
                .ValueGeneratedNever()
                .HasColumnName("calenholiday_id");
            entity.Property(e => e.CalenholidayDateEnd).HasColumnName("calenholiday_date_end");
            entity.Property(e => e.CalenholidayDateStart).HasColumnName("calenholiday_date_start");
            entity.Property(e => e.WorkerId).HasColumnName("worker_id");

            entity.HasOne(d => d.Worker).WithMany(p => p.CalendarWorkersHolidays)
                .HasForeignKey(d => d.WorkerId)
                .HasConstraintName("calendar_workers_holidays_worker_id_fkey");
        });

        modelBuilder.Entity<Ismain>(entity =>
        {
            entity.HasKey(e => e.IsmainId).HasName("ismain_pkey");

            entity.ToTable("ismain", "Voronzov2");

            entity.Property(e => e.IsmainId)
                .ValueGeneratedNever()
                .HasColumnName("ismain_id");
            entity.Property(e => e.IsmainName).HasColumnName("ismain_name");
        });

        modelBuilder.Entity<LearnAndWorker>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("learn_and_workers", "Voronzov2");

            entity.Property(e => e.LearnId).HasColumnName("learn_id");
            entity.Property(e => e.WorkerId).HasColumnName("worker_id");

            entity.HasOne(d => d.Learn).WithMany()
                .HasForeignKey(d => d.LearnId)
                .HasConstraintName("learn_and_workers_learn_id_fkey");

            entity.HasOne(d => d.Worker).WithMany()
                .HasForeignKey(d => d.WorkerId)
                .HasConstraintName("learn_and_workers_worker_id_fkey");
        });

        modelBuilder.Entity<LearnEvent>(entity =>
        {
            entity.HasKey(e => e.LearneventId).HasName("learn_events_pkey");

            entity.ToTable("learn_events", "Voronzov2");

            entity.Property(e => e.LearneventId)
                .ValueGeneratedNever()
                .HasColumnName("learnevent_id");
            entity.Property(e => e.LearneventDescription).HasColumnName("learnevent_description");
            entity.Property(e => e.LearneventName).HasColumnName("learnevent_name");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.OrganId).HasName("organizations_pkey");

            entity.ToTable("organizations", "Voronzov2");

            entity.Property(e => e.OrganId)
                .ValueGeneratedNever()
                .HasColumnName("organ_id");
            entity.Property(e => e.OrganName).HasColumnName("organ_name");
        });

        modelBuilder.Entity<OrganizationsHierarchy>(entity =>
        {
            entity.HasKey(e => e.HierId).HasName("organizations_hierarchy_pkey");

            entity.ToTable("organizations_hierarchy", "Voronzov2");

            entity.Property(e => e.HierId)
                .ValueGeneratedNever()
                .HasColumnName("hier_id");
            entity.Property(e => e.HierOrganId).HasColumnName("hier_organ_id");
            entity.Property(e => e.HierSuborganId).HasColumnName("hier_suborgan_id");
            entity.Property(e => e.HierSubsuborganId).HasColumnName("hier_subsuborgan_id");

            entity.HasOne(d => d.HierOrgan).WithMany(p => p.OrganizationsHierarchies)
                .HasForeignKey(d => d.HierOrganId)
                .HasConstraintName("organizations_hierarchy_hier_organ_id_fkey");

            entity.HasOne(d => d.HierSuborgan).WithMany(p => p.OrganizationsHierarchies)
                .HasForeignKey(d => d.HierSuborganId)
                .HasConstraintName("organizations_hierarchy_hier_suborgan_id_fkey");

            entity.HasOne(d => d.HierSubsuborgan).WithMany(p => p.OrganizationsHierarchies)
                .HasForeignKey(d => d.HierSubsuborganId)
                .HasConstraintName("organizations_hierarchy_hier_subsuborgan_id_fkey");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("posts_pkey");

            entity.ToTable("posts", "Voronzov2");

            entity.Property(e => e.PostId)
                .ValueGeneratedNever()
                .HasColumnName("post_id");
            entity.Property(e => e.PostIsprimId).HasColumnName("post_isprim_id");
            entity.Property(e => e.PostName).HasColumnName("post_name");

            entity.HasOne(d => d.PostIsprim).WithMany(p => p.Posts)
                .HasForeignKey(d => d.PostIsprimId)
                .HasConstraintName("posts_post_isprim_id_fkey");
        });

        modelBuilder.Entity<Suborganization>(entity =>
        {
            entity.HasKey(e => e.SuborganId).HasName("suborganizations_pkey");

            entity.ToTable("suborganizations", "Voronzov2");

            entity.Property(e => e.SuborganId)
                .ValueGeneratedNever()
                .HasColumnName("suborgan_id");
            entity.Property(e => e.SuborganDescription).HasColumnName("suborgan_description");
            entity.Property(e => e.SuborganName).HasColumnName("suborgan_name");
        });

        modelBuilder.Entity<Subsuborganization>(entity =>
        {
            entity.HasKey(e => e.SubsuborganId).HasName("subsuborganizations_pkey");

            entity.ToTable("subsuborganizations", "Voronzov2");

            entity.Property(e => e.SubsuborganId)
                .ValueGeneratedNever()
                .HasColumnName("subsuborgan_id");
            entity.Property(e => e.SubsuborganName).HasColumnName("subsuborgan_name");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.WorkerId).HasName("workers_pkey");

            entity.ToTable("workers", "Voronzov2");

            entity.Property(e => e.WorkerId)
                .ValueGeneratedNever()
                .HasColumnName("worker_id");
            entity.Property(e => e.WorkerBirtday).HasColumnName("worker_birtday");
            entity.Property(e => e.WorkerCabId).HasColumnName("worker_cab_id");
            entity.Property(e => e.WorkerEmail).HasColumnName("worker_email");
            entity.Property(e => e.WorkerFio).HasColumnName("worker_fio");
            entity.Property(e => e.WorkerOrganId).HasColumnName("worker_organ_id");
            entity.Property(e => e.WorkerPersonalphone).HasColumnName("worker_personalphone");
            entity.Property(e => e.WorkerPostId).HasColumnName("worker_post_id");
            entity.Property(e => e.WorkerWorkphone).HasColumnName("worker_workphone");

            entity.HasOne(d => d.WorkerCab).WithMany(p => p.Workers)
                .HasForeignKey(d => d.WorkerCabId)
                .HasConstraintName("workers_worker_cab_id_fkey");

            entity.HasOne(d => d.WorkerOrgan).WithMany(p => p.Workers)
                .HasForeignKey(d => d.WorkerOrganId)
                .HasConstraintName("workers_organizations_hierarchy_fk");

            entity.HasOne(d => d.WorkerPost).WithMany(p => p.Workers)
                .HasForeignKey(d => d.WorkerPostId)
                .HasConstraintName("workers_worker_post_id_fkey");
        });

        modelBuilder.Entity<Workingcalendar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("workingcalendar_pk");

            entity.ToTable("workingcalendar", "Voronzov2", tb => tb.HasComment("Список дней исключений в производственном календаре"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Exceptiondate)
                .HasComment("День-исключение")
                .HasColumnName("exceptiondate");
            entity.Property(e => e.Isworkingday)
                .HasComment("0 - будний день, но законодательно принят выходным; 1 - сб или вс, но является рабочим")
                .HasColumnName("isworkingday");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
