using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace NewsPaper.Gateway.Configure
{
    public class ConfigureCommon
    {
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, AutoMapper.IConfigurationProvider mapper)
        {
            if (env.IsDevelopment())
            {
                mapper.AssertConfigurationIsValid();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                mapper.CompileMappings();
            }
        }
    }
}
