using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NewsPaper.Gateway.ConfigureServices
{
    public class ConfigureServicesBase
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(config =>
            {
                config.AddPolicy("DefaultPolicy",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddAutoMapper(typeof(Startup));
        }
    }
}
