using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PFM.API.Data;
using PFM.API.Models.Entities;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureDatabaseExt
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var migrationAssembly = typeof(PFM.API.Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<AppDbContext>(o =>
                o.UseSqlServer(configuration.GetConnectionString("Default"),
                sql => sql.MigrationsAssembly(migrationAssembly)));

            services.AddIdentity<AppUser, IdentityRole>(o =>
            {
                o.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
