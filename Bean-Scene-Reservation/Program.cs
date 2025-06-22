using Bean_Scene_Reservation.Data;
using Bean_Scene_Reservation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bean_Scene_Reservation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Logging providers
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // Add services to the container.

            // Add localisation services
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "en-AU" };
                options.SetDefaultCulture(supportedCultures[0])
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);
            });

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Add Indentity support in out app (using our custom ApplicationUser class)
            builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
                options.SignIn.RequireConfirmedAccount = true;

                // Change password complexity requirements
                options.Password.RequiredLength = 8;
                //options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequireLowercase = true;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireDigit = true;
            })
            .AddRoles<IdentityRole>() // Add support for roles 
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add other services
            builder.Services.AddControllersWithViews();

            // Build the app
            var app = builder.Build();

            // Use request localization
            app.UseRequestLocalization();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
