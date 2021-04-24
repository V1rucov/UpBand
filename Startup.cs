using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using upband.Data;
using upband.Data.Entities;

namespace upband
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication().AddCookie(options => {
                options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Registration");
            });
            services.AddDbContext<DataBaseContext>(options => {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddIdentity<user, IdentityRole>(opts=> {
                //password
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireDigit = false;
                opts.Password.RequireUppercase = false;
                //account
                opts.User.RequireUniqueEmail = false;
                opts.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<DataBaseContext>()
                .AddDefaultTokenProviders();
            services.AddControllersWithViews();

            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IPasswordHasher<user>, SHA512Service>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
            });
        }
    }
}