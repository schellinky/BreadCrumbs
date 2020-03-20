using System;
using BreadCrumbs.Areas.Identity.Data;
using BreadCrumbs.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(BreadCrumbs.Areas.Identity.IdentityHostingStartup))]
namespace BreadCrumbs.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        private string _connection = null;
        public void Configure(IWebHostBuilder builder)
        {
            if (builder != null)
            {
                builder.ConfigureServices((context, services) =>
                {
                    var builder1 = new SqlConnectionStringBuilder(
                    context.Configuration.GetConnectionString("IdentityContextConnection"));
                    builder1.Password = context.Configuration["DbPassword"];
                    _connection = builder1.ConnectionString;
                    services.AddDbContext<IdentityContext>(options =>
                        options.UseSqlServer(
                            _connection));


                    services.AddDefaultIdentity<BreadCrumbsUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
                        .AddEntityFrameworkStores<IdentityContext>();
                });
            }
        }
    }
}