# Newspaper.GateWay

This is a .NET Core 3.1 API gateway, connected to MassTransit for working with RabbitMQ using the AMQP protocol,
connected to IdentityServer4 to check the validity of the token, communicating with clients using the HTTP protocol, and there is also a response wrapper in OperationResult.
Using AutoMapper to map DTO Model to ViewModel.

## Requests && Response in GateWay

Uses Multiple Response Types to get 100% response from the microservice, regardless of the result of the operation. contains a positive response model and a failure response model.

> The server's response is always clear to the user.

Since an OperationResult object arrives, which contains the `Result`, `Exception`, `Logs`, `Metadata` fields. 
And when accessing various atomic microservices, keep a convenient log of errors and informative messages.
```C#
[Authorize]
[HttpGet("getarticlebyid")]
public async Task<IActionResult> GetArticleById(Guid articleGuid)
{
    var operation = OperationResult.CreateResult<ArticleViewModel>();
    var (statusResponse, notFoundResponse) =
        await _requestClientArticleById.GetResponse<ArticleResponseDto, NoArticlesFound>(new ArticleByIdRequestDto
    {
        ArticleGuid = articleGuid
    });
    if (statusResponse.IsCompletedSuccessfully)
    {
        operation.Result = _mapper.Map<ArticleViewModel>(statusResponse.Result.Message.ArticleDto);
        return Ok(operation);
    }
    operation.AddError(new Exception(notFoundResponse.Result.Message.MassageException));
    var output = JsonConvert.SerializeObject(operation);

    return Ok(output);
}
```

## ConfigureServices MassTransit for RabbitMq

RequestClient is added to MassTransit in ConfigureServicesMassTransitRabbitMq

```C#
public class ConfigureServicesMassTransitRabbitMQ
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
```

## Links to project repositories
- :white_check_mark:[NewsPaper Review](https://github.com/PKravchenko-ki16/NewsPaper)
- :white_check_mark:[NewsPaper.MassTransit.Configuration](https://github.com/PKravchenko-ki16/NewsPaper.MassTransit.Configuration)
- :white_check_mark:[NewsPaper.MassTransit.Contracts](https://github.com/PKravchenko-ki16/NewsPaper.MassTransit.Contracts)
- :black_square_button:[NewsPaper.IdentityServer]()
- :white_check_mark:Newspaper.GateWay
- :white_check_mark:[NewsPaper.Accounts](https://github.com/PKravchenko-ki16/NewsPaper.Accounts)
- :white_check_mark:[NewsPaper.Articles](https://github.com/PKravchenko-ki16/NewsPaper.Articles)
- :white_check_mark:[NewsPaper.GatewayClientApi](https://github.com/PKravchenko-ki16/NewsPaper.GatewayClientApi)
- :white_check_mark:[NewsPaper.Client.Mvc](https://github.com/PKravchenko-ki16/NewsPaper.Client.Mvc)
