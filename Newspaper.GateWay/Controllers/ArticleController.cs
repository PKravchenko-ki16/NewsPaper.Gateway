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
using NewsPaper.MassTransit.Contracts.DTO.Models.Articles;
using NewsPaper.MassTransit.Contracts.DTO.Requests.Articles;
using NewsPaper.MassTransit.Contracts.DTO.Responses.Articles;
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

        private readonly IMapper _mapper;
        public ArticleController(IMapper mapper, IRequestClient<ArticlesByIdAuthorRequestDto> requestClientArticlesByIdAuthor, IRequestClient<ArticleByIdRequestDto> requestClientArticleById, IRequestClient<ArticlesRequestDto> requestClientArticles, IRequestClient<ArticleCreateRequestDto> requestClientArticleCreate, IRequestClient<ArticleGoArchiveRequestDto> requestClientArticleGoArchive)
        {
            _mapper = mapper;
            _requestClientArticlesByIdAuthor = requestClientArticlesByIdAuthor;
            _requestClientArticleById = requestClientArticleById;
            _requestClientArticles = requestClientArticles;
            _requestClientArticleCreate = requestClientArticleCreate;
            _requestClientArticleGoArchive = requestClientArticleGoArchive;
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
            var operation = OperationResult.CreateResult<Guid>();
            var articleDto = _mapper.Map<ArticleDto>(articleViewModel);
            var (statusResponse, failedToCreateArticleResponse) =
                await _requestClientArticleCreate.GetResponse<ArticleCreateResponseDto, FailedToCreateArticle>(new ArticleCreateRequestDto
                {
                    Article = articleDto
                });
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = statusResponse.Result.Message.ArticleGuid;
                return Ok(operation);
            }
            operation.AddError(new Exception(failedToCreateArticleResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
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
                operation.Result = statusResponse.Result.Message.ArticleGuid;
                return Ok(operation);
            }
            operation.AddError(new Exception(failedTransferToArchiveResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }
    }
}
