using System;
using Microsoft.Extensions.DependencyInjection;
using DirectorySite.Data;
using DirectorySite.Services;

namespace DirectorySite.Helper
{
    public static class PreregisterServiceCollectionExtension
    {
        public static void AddPreregisterServices(this IServiceCollection services)
        {
            services.AddScoped<PreregisterService>();
        }
    }
}