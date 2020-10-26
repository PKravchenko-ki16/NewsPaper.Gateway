using System;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NewsPaper.Gateway.ConfigureServices
{
    public class ConfigureServicesMassTransit
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var massTransitSection = configuration.GetSection("MassTransit");
            var url = massTransitSection.GetValue<string>("Url");
            var host = massTransitSection.GetValue<string>("Host");
            var userName = massTransitSection.GetValue<string>("UserName");
            var password = massTransitSection.GetValue<string>("Password");
            if (massTransitSection == null || url == null || host == null)
            {
                throw new Exception("Section 'mass-transit' configuration settings are not found in appSettings.json");
            }

            services.AddMassTransit(x =>
            {
                x.AddBus(busFactory =>
                {
                    var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host($"rabbitmq://{url}/{host}", configurator =>
                        {
                            configurator.Username(userName);
                            configurator.Password(password);
                        });

                        cfg.ConfigureEndpoints(busFactory, KebabCaseEndpointNameFormatter.Instance);

                        cfg.UseJsonSerializer();

                        cfg.UseHealthCheck(busFactory);
                    });

                    return bus;
                });

            });

            services.AddMassTransitHostedService();
        }
    }
}