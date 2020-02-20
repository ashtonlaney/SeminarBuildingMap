using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeminarBuildingMap.Areas.Identity.Data;
using SeminarBuildingMap.Data;

[assembly: HostingStartup(typeof(SeminarBuildingMap.Areas.Identity.IdentityHostingStartup))]
namespace SeminarBuildingMap.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {

            builder.ConfigureServices((context, services) => {
                services.AddDbContext<SeminarBuildingMapContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("SeminarBuildingMapContextConnection")));

                services.AddDefaultIdentity<SeminarBuildingMapUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<SeminarBuildingMapContext>();
            });
        }
    }
}