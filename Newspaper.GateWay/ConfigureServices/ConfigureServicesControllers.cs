using Microsoft.Extensions.DependencyInjection;

namespace NewsPaper.GateWay.ConfigureServices
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
