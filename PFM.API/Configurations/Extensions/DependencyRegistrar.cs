using Microsoft.Extensions.Configuration;
using PFM.API.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyRegistrar
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddTransient<IJwtFactory, JwtFactory>();
            services.AddTransient<IJwtTokenHandler, JwtTokenHandler>();

            return services;
        }
    }
}
