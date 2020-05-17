
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.EntityModels
{
    public class s19299Context : DbContext
    {
        
        public s19299Context()
        {
        }

        public s19299Context(DbContextOptions<s19299Context> options)
            : base(options)
        {
        }
        
        public virtual DbSet<Enrollment> Enrollment { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Course> Courses { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=db-mssql;Initial Catalog=s19299;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.IdEnrollment)
                    .HasName("Enrollment_pk");

                entity.Property(e => e.IdEnrollment).ValueGeneratedNever();

                entity.Property(e => e.startDate).HasColumnType("date");
                
                
                
                modelBuilder.Entity<Student>(entity =>
                {
                    entity.HasKey(e => e.StudentID)
                        .HasName("Student_pk");

                    entity.Property(e => e.StudentID).HasMaxLength(50);

                    entity.Property(e => e.birthDate).HasColumnType("date");

                    entity.Property(e => e.firstName)
                        .IsRequired()
                        .HasMaxLength(100);

                    entity.Property(e => e.lastName)
                        .IsRequired()
                        .HasMaxLength(100);

                });

                modelBuilder.Entity<Course>(entity =>
                {
                    entity.HasKey(e => e.IdStudy).HasName("Studies_pk");

                    entity.Property(e => e.IdStudy).IsRequired();

                    entity.Property(e => e.Name).IsRequired().HasMaxLength(50);

                });



                OnModelCreatingPartial(modelBuilder);
            });
        }

        protected void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
        }
    }
    
}