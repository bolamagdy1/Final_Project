using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Final_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>
            (
            options => options
            .UseSqlServer("Server=.;Database=final_project;Trusted_connection=true;TrustServerCertificate=true")
            );

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            builder.Services.AddMemoryCache();

            builder.Services.AddSession();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Applicant/Login";
                //options.AccessDeniedPath = "/Account/AccessDenied";
            });

            builder.Services.AddSingleton(x =>
            new PaypalClient(
            builder.Configuration["PayPalOptions:ClientId"],
            builder.Configuration["PayPalOptions:ClientSecret"],
            builder.Configuration["PayPalOptions:Mode"]
        )
    );
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseCookiePolicy();
            
            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            AppDbInitializer.SeedRolesToDatabase(app).Wait();

            app.Run();

            
        }
    }
}