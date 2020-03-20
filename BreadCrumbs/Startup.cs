using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BreadCrumbs.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using BreadCrumbs.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace BreadCrumbs
{
    public class Startup
    {
        private string _connection = null;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            /*services.AddDbContext<TicketContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));*/

            var builder = new SqlConnectionStringBuilder(
                Configuration.GetConnectionString("DevConnection"));
            builder.Password = Configuration["DbPassword"];
            _connection = builder.ConnectionString;
            services.AddDbContext<TicketContext>(options => options.UseSqlServer(_connection));






            /*var builder1 = new SqlConnectionStringBuilder(
                Configuration.GetConnectionString("IdentityContextConnection"));
            builder1.Password = Configuration["DbPassword"];
            _connection = builder1.ConnectionString;*/
            /*
            services.AddDbContext<IdentityContext>(options =>
            options.UseSqlServer(_connection));*/

            /*services.AddDbContextPool<TicketContext>(
                options => options.UseSqlServer(_connection));*/
            /*services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>();*/

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
