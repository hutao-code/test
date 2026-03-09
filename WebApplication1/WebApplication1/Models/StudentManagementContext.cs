using System.Data.Entity;

namespace WebApplication1.Models
{
    public class StudentManagementContext : DbContext
    {
        public StudentManagementContext() : base("name=StudentManagementConnection")
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<CourseRegistration> CourseRegistrations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Student 配置
            modelBuilder.Entity<Student>()
                .HasKey(s => s.StudentId);

            modelBuilder.Entity<Student>()
                .Property(s => s.StudentId)
                .HasMaxLength(8)
                .IsRequired();

            modelBuilder.Entity<Student>()
                .Property(s => s.Name)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Student>()
                .Property(s => s.Major)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Student>()
                .Property(s => s.Grade)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<Student>()
                .Property(s => s.Email)
                .HasMaxLength(100);

            modelBuilder.Entity<Student>()
                .Property(s => s.Phone)
                .HasMaxLength(20);

            // 为 Student 添加索引以优化查询性能
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Name)
                .HasName("IX_Students_Name");

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Grade)
                .HasName("IX_Students_Grade");

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Major)
                .HasName("IX_Students_Major");

            // Course 配置
            modelBuilder.Entity<Course>()
                .HasKey(c => c.CourseId);

            modelBuilder.Entity<Course>()
                .Property(c => c.CourseId)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<Course>()
                .Property(c => c.CourseName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Course>()
                .Property(c => c.Instructor)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Course>()
                .Property(c => c.Description)
                .HasMaxLength(500);

            // Grade 配置
            modelBuilder.Entity<Grade>()
                .HasKey(g => g.GradeId);

            modelBuilder.Entity<Grade>()
                .Property(g => g.Score)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Grade>()
                .HasRequired(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Grade>()
                .HasRequired(g => g.Course)
                .WithMany(c => c.Grades)
                .HasForeignKey(g => g.CourseId)
                .WillCascadeOnDelete(true);

            // 确保学生-课程组合唯一
            modelBuilder.Entity<Grade>()
                .HasIndex(g => new { g.StudentId, g.CourseId })
                .IsUnique();

            // 为 Grade 添加索引以优化查询性能
            modelBuilder.Entity<Grade>()
                .HasIndex(g => g.StudentId)
                .HasName("IX_Grades_StudentId");

            modelBuilder.Entity<Grade>()
                .HasIndex(g => g.CourseId)
                .HasName("IX_Grades_CourseId");

            // CourseRegistration 配置
            modelBuilder.Entity<CourseRegistration>()
                .HasKey(cr => cr.RegistrationId);

            modelBuilder.Entity<CourseRegistration>()
                .HasRequired(cr => cr.Student)
                .WithMany(s => s.CourseRegistrations)
                .HasForeignKey(cr => cr.StudentId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<CourseRegistration>()
                .HasRequired(cr => cr.Course)
                .WithMany(c => c.CourseRegistrations)
                .HasForeignKey(cr => cr.CourseId)
                .WillCascadeOnDelete(true);

            // 确保学生-课程组合唯一
            modelBuilder.Entity<CourseRegistration>()
                .HasIndex(cr => new { cr.StudentId, cr.CourseId })
                .IsUnique();

            // 为 CourseRegistration 添加索引以优化查询性能
            modelBuilder.Entity<CourseRegistration>()
                .HasIndex(cr => cr.StudentId)
                .HasName("IX_CourseRegistrations_StudentId");

            modelBuilder.Entity<CourseRegistration>()
                .HasIndex(cr => cr.CourseId)
                .HasName("IX_CourseRegistrations_CourseId");
        }
    }
}
