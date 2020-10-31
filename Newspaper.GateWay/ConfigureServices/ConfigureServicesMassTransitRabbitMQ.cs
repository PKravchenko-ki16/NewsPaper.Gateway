using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsPaper.MassTransit.Configuration;
using NewsPaper.MassTransit.Contracts.DTO.Exception.Articles;
using NewsPaper.MassTransit.Contracts.DTO.Requests.Articles;
using ConfigureServicesMassTransit = NewsPaper.MassTransit.Configuration.ConfigureServicesMassTransit;

namespace NewsPaper.Gateway.ConfigureServices
{
    public class ConfigureServicesMassTransitRabbitMq
    {
        public static void ConfigureService(IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("MassTransit");
            ConfigureServicesMassTransit.ConfigureServices(services, configuration, new MassTransitConfiguration()
            {
                IsDebug = section.GetValue<bool>("IsDebug"),
                ServiceName = "Gateway",
                Configurator = busMassTransit =>
                {
                    busMassTransit.AddRequestClient<ArticlesByIdAuthorRequestDto>(); 
                }
            });
        }
    }
}
