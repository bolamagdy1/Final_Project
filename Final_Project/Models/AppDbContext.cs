using Microsoft.EntityFrameworkCore;

namespace Final_Project.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext()
        {
            
        }
        public AppDbContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<Applicant> applicants { get; set; }
        public DbSet<Applicant_Course> As_Cs { get; set; }
        public DbSet<Applicant_Job> applicants_jobs { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<Course> courses { get; set; }
        public DbSet<Course_Videos> c_vs { get; set; }
        public DbSet<Job> jobs { get; set; }
    }
}
