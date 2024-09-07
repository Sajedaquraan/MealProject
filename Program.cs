using MealProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;

namespace MealProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ModelContext>(options => options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                //options.Cookie.HttpOnly = true;
                //options.Cookie.IsEssential = true;
                //options.Cookie.Name = "MyCustomSessionCookie";
                //options.Cookie.Path = "/"; // Ensure this covers all paths
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
             .AddCookie(options =>
             {
                 options.LoginPath = "/Account/Login";
                 options.LogoutPath = "/Account/Logout";
             })
             .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
             {
                 options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
                 options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;


             });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}