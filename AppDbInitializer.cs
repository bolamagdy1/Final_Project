using Final_Project.Models;
using Microsoft.AspNetCore.Identity;

namespace Final_Project.Data
{
    public class AppDbInitializer
    {
        public AppDbContext _context;
        public AppDbInitializer(AppDbContext context)
        {
            _context = context;
        }
        public static async Task SeedRolesToDatabase(IApplicationBuilder applicationBuilder)
        {
            using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope()) 
            {
                //var _context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                //_context.Database.EnsureCreated();

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if(!await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                }
                if (!await roleManager.RoleExistsAsync(UserRoles.Applicant))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Applicant));
                }
                if (!await roleManager.RoleExistsAsync(UserRoles.Company))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Company));
                }

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var adminUser = await userManager.FindByEmailAsync("Admin@je_journey.com");
                if (adminUser == null)
                {
                    var newAdmin = new ApplicationUser
                    {
                        FullName = "Admin User",
                        UserName = "Admin",
                        Email = "Admin@je_journey.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdmin,"Admin@1234");
                    await userManager.AddToRoleAsync(newAdmin, UserRoles.Admin);
                }

                var applicantUser = await userManager.FindByEmailAsync("Applicant@je_journey.com");
                if (applicantUser == null)
                {
                    var newApplicant = new ApplicationUser
                    {
                        FullName = "Applicant User",
                        UserName = "Applicant",
                        Email = "Applicant@je_journey.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newApplicant, "Applicant@1234");
                    await userManager.AddToRoleAsync(newApplicant, UserRoles.Applicant);
                }
               
                var companyUser = await userManager.FindByEmailAsync("Company@je_journey.com");
                if (companyUser == null)
                {
                    var newCompany = new ApplicationUser
                    {
                        FullName = "Company User",
                        UserName = "Company",
                        Email = "Company@je_journey.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newCompany, "Company@1234");
                   await userManager.AddToRoleAsync(newCompany, UserRoles.Company);   
                }
            }
        } 
    }
}
