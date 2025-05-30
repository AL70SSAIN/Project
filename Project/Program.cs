using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.DataBase;
using Project.Identity;
using Project.ViewModels;

namespace Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSession();

            builder.Services.AddControllersWithViews();

            string ConnectionString = builder.Configuration.GetConnectionString("Project");

            builder.Services.AddDbContext<Context>(OptionsBuilder =>
            {
                OptionsBuilder.UseSqlServer(ConnectionString);
            });

            builder.Services.AddControllersWithViews();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<Context>();

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

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
