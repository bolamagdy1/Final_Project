using Final_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Applicant> applicants { get; set; }
        public DbSet<Applicant_Course> As_Cs { get; set; }
        public DbSet<Applicant_Job> applicants_jobs { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<Course> courses { get; set; }
        public DbSet<Course_Videos> c_vs { get; set; }
        public DbSet<Job> jobs { get; set; }
        public DbSet<Accepted> accepted { get; set; }
        public DbSet<Final_Project.Models.Meeting> Meeting { get; set; } = default!;
    }
}
