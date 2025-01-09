using Microsoft.EntityFrameworkCore;

namespace MyDoctorApp.Data
{
    public class MyDoctorAppDbContext : DbContext
    {
        protected MyDoctorAppDbContext()
        {
        }

        public MyDoctorAppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(e => e.Username).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.Password).HasMaxLength(60);
                entity.Property(e => e.Firstname).HasMaxLength(50);
                entity.Property(e => e.Lastname).HasMaxLength(100);
                entity.Property(e => e.UserRole).HasMaxLength(255).HasConversion<string>();

                entity.Property(e => e.InsertedAt).
                ValueGeneratedOnAdd().
                HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.ModifiedAt).
                ValueGeneratedOnAddOrUpdate().
                HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.Lastname, "IX_Users_Lastname");
                entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();
                entity.HasIndex(e => e.Username, "IX_Users_Username").IsUnique();

            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patients");
                entity.Property(e => e.Amka).HasMaxLength(15);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.InsertedAt).
                ValueGeneratedOnAdd().
                HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.ModifiedAt).
                ValueGeneratedOnAddOrUpdate().
                HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.City, "IX_Patients_City");
                entity.HasIndex(e => e.Amka, "IX_Patients_Amka").IsUnique();
                entity.HasIndex(e => e.UserId, "IX_Patients_UserId").IsUnique();

                entity.HasMany(p => p.Doctors)
                    .WithMany(d => d.Patients)
                    .UsingEntity<Dictionary<string, object>>(
                        "PatientsDoctors",
                        j => j.HasOne<Doctor>()
                            .WithMany()
                            .HasForeignKey("DoctorId")
                            .OnDelete(DeleteBehavior.Restrict),
                        j => j.HasOne<Patient>()
                            .WithMany()
                            .HasForeignKey("PatientId")
                            .OnDelete(DeleteBehavior.Restrict)
                    );

            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctors");
                entity.Property(e => e.Afm).HasMaxLength(15);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.DoctorSpecialty).HasColumnName("Specialty").HasMaxLength(255).HasConversion<string>();

                entity.Property(e => e.InsertedAt).
                ValueGeneratedOnAdd().
                HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.ModifiedAt).
                ValueGeneratedOnAddOrUpdate().
                HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.City, "IX_Doctors_City");
                entity.HasIndex(e => e.Afm, "IX_Doctors_Afm").IsUnique();
                entity.HasIndex(e => e.PhoneNumber, "IX_Doctors_PhoneNumber").IsUnique();
                entity.HasIndex(e => e.UserId, "IX_Doctors_UserId").IsUnique();

            });
        }
    }
}
