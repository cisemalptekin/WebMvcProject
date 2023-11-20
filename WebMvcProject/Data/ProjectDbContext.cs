using Microsoft.EntityFrameworkCore;
using WebMvcProject.Data.Config;

namespace WebMvcProject.Data
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.UserName).IsUnique();
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<User>().Property(x => x.UserName).HasMaxLength(12);
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.ApplyConfiguration(new RoleConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}