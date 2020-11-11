using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Calabonga.OperationResults;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newspaper.GateWay.ViewModels.ViewModels.Article;
using NewsPaper.MassTransit.Contracts.DTO.Exception.Articles;
using NewsPaper.MassTransit.Contracts.DTO.Exception.Operation;
using NewsPaper.MassTransit.Contracts.DTO.Models.Articles;
using NewsPaper.MassTransit.Contracts.DTO.Requests.Articles;
using NewsPaper.MassTransit.Contracts.DTO.Requests.Operation;
using NewsPaper.MassTransit.Contracts.DTO.Responses.Articles;
using NewsPaper.MassTransit.Contracts.DTO.Responses.Operation;
using Newtonsoft.Json;

namespace NewsPaper.GateWay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IRequestClient<ArticlesByIdAuthorRequestDto> _requestClientArticlesByIdAuthor;
        private readonly IRequestClient<ArticleByIdRequestDto> _requestClientArticleById;
        private readonly IRequestClient<ArticlesRequestDto> _requestClientArticles;
        private readonly IRequestClient<ArticleCreateRequestDto> _requestClientArticleCreate;
        private readonly IRequestClient<ArticleGoArchiveRequestDto> _requestClientArticleGoArchive;
        private readonly IRequestClient<AccountsForCreateArticleRequestDto> _requestAccountsForCreateArticle;

        private readonly IMapper _mapper;
        public ArticleController(IMapper mapper, IRequestClient<ArticlesByIdAuthorRequestDto> requestClientArticlesByIdAuthor, IRequestClient<ArticleByIdRequestDto> requestClientArticleById, IRequestClient<ArticlesRequestDto> requestClientArticles, IRequestClient<ArticleCreateRequestDto> requestClientArticleCreate, IRequestClient<ArticleGoArchiveRequestDto> requestClientArticleGoArchive, IRequestClient<AccountsForCreateArticleRequestDto> requestAccountsForCreateArticle)
        {
            _mapper = mapper;
            _requestClientArticlesByIdAuthor = requestClientArticlesByIdAuthor;
            _requestClientArticleById = requestClientArticleById;
            _requestClientArticles = requestClientArticles;
            _requestClientArticleCreate = requestClientArticleCreate;
            _requestClientArticleGoArchive = requestClientArticleGoArchive;
            _requestAccountsForCreateArticle = requestAccountsForCreateArticle;
        }

        [Authorize]
        [HttpGet("getarticlesbyidauthor")]
        public async Task<IActionResult> GetArticlesByIdAuthor(Guid authorGuid)
        {
            var operation = OperationResult.CreateResult<IEnumerable<ArticleViewModel>>();
            var (statusResponse, notFoundResponse) =
                await _requestClientArticlesByIdAuthor.GetResponse<ArticlesByIdAuthorResponseDto, NoArticlesFound>(new ArticlesByIdAuthorRequestDto
                {
                    AuthorGuid = authorGuid
                });
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = _mapper.Map<IEnumerable<ArticleViewModel>>(statusResponse.Result.Message.ArticlesDto);
                return Ok(operation);
            }
            operation.AddError(new Exception(notFoundResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }

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

        [Authorize]
        [HttpGet("getarticles")]
        public async Task<IActionResult> GetArticles()
        {
            var operation = OperationResult.CreateResult<IEnumerable<ArticleViewModel>>();
            var (statusResponse, notFoundResponse) =
                await _requestClientArticles.GetResponse<ArticlesResponseDto, NoArticlesFound>(new ArticlesRequestDto());
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = _mapper.Map<IEnumerable<ArticleViewModel>>(statusResponse.Result.Message.ArticlesDto);
                return Ok(operation);
            }
            operation.AddError(new Exception(notFoundResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }

        [Authorize]
        [HttpPost("createarticle")]
        public async Task<IActionResult> CreateArticle(ArticleViewModel articleViewModel)
        {
            var operation = OperationResult.CreateResult<ArticleViewModel>();
            var (statusResponseAccounts, failedGetAccountsToCreateArticleResponse) =
                await _requestAccountsForCreateArticle.GetResponse<AccountsForCreateArticleResponseDto, FailedGetAccountsToCreateArticle>(new AccountsForCreateArticleRequestDto
                {
                    AuthorIdentityId = articleViewModel.AuthorGuid
                });
            if (statusResponseAccounts.IsCompletedSuccessfully)
            {
                articleViewModel.NikeNameAuthor = statusResponseAccounts.Result.Message.Author.NikeName;
                articleViewModel.EditorGuid = statusResponseAccounts.Result.Message.Editor.Id;
                articleViewModel.NikeNameEditor = statusResponseAccounts.Result.Message.Editor.NikeName;
                articleViewModel.DateOfWriting = DateTime.Now;
            }
            else
            {
                operation.AddError(new Exception(failedGetAccountsToCreateArticleResponse.Result.Message.MassageException));
                var output = JsonConvert.SerializeObject(operation);
                return Ok(output);
            }
            var articleDto = _mapper.Map<ArticleDto>(articleViewModel);
            var (statusResponse, failedToCreateArticleResponse) =
                await _requestClientArticleCreate.GetResponse<ArticleCreateResponseDto, FailedToCreateArticle>(new ArticleCreateRequestDto
                {
                    Article = articleDto
                });
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = _mapper.Map<ArticleViewModel>(statusResponse.Result.Message.Article);
                return Ok(operation);
            }
            else
            {
                operation.AddError(new Exception(failedToCreateArticleResponse.Result.Message.MassageException));
                var output = JsonConvert.SerializeObject(operation);
                return Ok(output);
            }
        }

        [Authorize]
        [HttpGet("goarchivearticle")]
        public async Task<IActionResult> GoArchiveArticle(Guid articleGuid)
        {
            var operation = OperationResult.CreateResult<Guid>();
            var (statusResponse, failedTransferToArchiveResponse) =
                await _requestClientArticleGoArchive.GetResponse<ArticleGoArchiveResponseDto, FailedTransferToArchive>(new ArticleGoArchiveRequestDto
                {
                    ArticleGuid = articleGuid
                });
            if (statusResponse.IsCompletedSuccessfully)
            {
                return Ok(operation);
            }
            operation.AddError(new Exception(failedTransferToArchiveResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }
    }
}
