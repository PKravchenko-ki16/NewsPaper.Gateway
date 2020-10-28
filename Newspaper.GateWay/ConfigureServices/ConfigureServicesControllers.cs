using Microsoft.Extensions.DependencyInjection;

namespace NewsPaper.Gateway.ConfigureServices
{
    public class ConfigureServicesControllers
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers();
            services.AddControllersWithViews();
        }
    }
}
