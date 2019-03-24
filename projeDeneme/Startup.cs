using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.Extensions.DependencyInjection;
using projeDeneme.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Devart.Data.MySql.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using projeDeneme.Identity;

namespace projeDeneme
{
    public class Startup
    {
        private IConfiguration _configiration;


        public Startup(IConfiguration configuration)
        {
            _configiration = configuration;
        }
       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ModelmysqlContext>(options => options.UseSqlite(_configiration["ConnectionStrings"]));
            //services.AddDbContext<ModelmysqlContext>(options => options.UseMySql(_configiration["DBConStr"]);
           services.AddDbContext<AppIdentityDbContex>(options => options.UseSqlite(_configiration["ConnectionStrings"]));
            //services.AddDbContext<AppIdentityDbContex>(options => options.UseMySql(_configiration["DBConStr"]);
            services.AddIdentity<AppIdentityUser, AppIdentityRole>().AddEntityFrameworkStores<AppIdentityDbContex>()
                .AddDefaultTokenProviders();
           
           


            services.Configure<IdentityOptions>(options => 
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Security/Login";
                options.LogoutPath = "/Security/Logout";
                options.AccessDeniedPath = "/Security/AccessDenied";
                options.SlidingExpiration = true;
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = "cookie",
                    Path="/",
                    SameSite=SameSiteMode.Lax,
                    SecurePolicy=CookieSecurePolicy.SameAsRequest
            };
            });
            
            services.AddMvc();
            services.AddSession();

        }
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var rolManager = serviceProvider.GetRequiredService<RoleManager<AppIdentityRole>>();
            IdentityResult roleRuselt;
            var rolCheck= await rolManager.RoleExistsAsync("Admin");
            if (!rolCheck)
            {
                roleRuselt = await rolManager.CreateAsync(new AppIdentityRole("Admin"));

            }
            rolCheck = await rolManager.RoleExistsAsync("Kullanici");
            if (!rolCheck)
            {
                roleRuselt = await rolManager.CreateAsync(new AppIdentityRole("Kullanici"));

            }
            rolCheck = await rolManager.RoleExistsAsync("Musteri");
            if (!rolCheck)
            {
                roleRuselt = await rolManager.CreateAsync(new AppIdentityRole("Musteri"));

            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //env.EnvironmentName = EnvironmentName.Production;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(ConfigureRoutes);
            app.UseStaticFiles();
            

        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(
                  name:"default",
                 template: "{controller=home}/{action=index}/{id?}"
                  );
            routeBuilder.MapRoute
                ( name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

        }
    }
}
//routes.

