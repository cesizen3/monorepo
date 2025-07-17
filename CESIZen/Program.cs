using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CESIZen.Models;
using CesiZen.Data;

namespace CESIZen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<CesiZenDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // Identity
            builder.Services.AddIdentity<Utilisateur, IdentityRole<int>>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<CesiZenDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "admin",
                pattern: "Admin/{controller=User}/{action=Index}/{id?}",
                defaults: new { area = "" });

            // Seed roles
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<Utilisateur>>();

                string[] roleNames = { "Admin", "User" };

                foreach (var roleName in roleNames)
                {
                    if (!roleManager.RoleExistsAsync(roleName).Result)
                    {
                        roleManager.CreateAsync(new IdentityRole<int>(roleName)).Wait();
                    }
                }
            }

            app.Run();
        }
    }
}
