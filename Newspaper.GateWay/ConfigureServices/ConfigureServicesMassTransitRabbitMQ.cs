using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsPaper.MassTransit.Configuration;
using NewsPaper.MassTransit.Contracts.DTO.Requests.Articles;
using NewsPaper.MassTransit.Contracts.DTO.Requests.Author;
using NewsPaper.MassTransit.Contracts.DTO.Requests.Editor;
using NewsPaper.MassTransit.Contracts.DTO.Requests.Operation;
using NewsPaper.MassTransit.Contracts.DTO.Requests.User;
using NewsPaper.MassTransit.Contracts.DTO.Responses.Operation;
using ConfigureServicesMassTransit = NewsPaper.MassTransit.Configuration.ConfigureServicesMassTransit;

namespace NewsPaper.GateWay.ConfigureServices
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
                    busMassTransit.AddRequestClient<ArticleByIdRequestDto>();
                    busMassTransit.AddRequestClient<ArticlesRequestDto>();
                    busMassTransit.AddRequestClient<ArticleCreateRequestDto>();
                    busMassTransit.AddRequestClient<ArticleGoArchiveRequestDto>();

                    busMassTransit.AddRequestClient<AuthorRequestDto>();
                    busMassTransit.AddRequestClient<AuthorsRequestDto>();
                    busMassTransit.AddRequestClient<GuidAuthorRequestDto>();
                    busMassTransit.AddRequestClient<NikeNameAuthorRequestDto>();

                    busMassTransit.AddRequestClient<EditorRequestDto>();
                    busMassTransit.AddRequestClient<EditorsRequestDto>();
                    busMassTransit.AddRequestClient<GuidEditorRequestDto>();
                    busMassTransit.AddRequestClient<NikeNameEditorRequestDto>();

                    busMassTransit.AddRequestClient<UserRequestDto>();
                    busMassTransit.AddRequestClient<UsersRequestDto>();
                    busMassTransit.AddRequestClient<GuidUserRequestDto>();
                    busMassTransit.AddRequestClient<NikeNameUserRequestDto>();

                    busMassTransit.AddRequestClient<AccountsForCreateArticleRequestDto>();
                }
            });
        }
    }
}
