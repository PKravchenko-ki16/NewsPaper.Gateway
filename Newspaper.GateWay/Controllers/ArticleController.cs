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

        private readonly IMapper _mapper;
        public ArticleController(IMapper mapper, IRequestClient<ArticlesByIdAuthorRequestDto> requestClientArticlesByIdAuthor, IRequestClient<ArticleByIdRequestDto> requestClientArticleById, IRequestClient<ArticlesRequestDto> requestClientArticles)
        {
            _mapper = mapper;
            _requestClientArticlesByIdAuthor = requestClientArticlesByIdAuthor;
            _requestClientArticleById = requestClientArticleById;
            _requestClientArticles = requestClientArticles;
        }

        [Authorize]
        [HttpGet("getarticlesbyidauthor")]
        public async Task<IActionResult> GetArticlesByIdAuthor(Guid authorGuid)
        {
            var operation = OperationResult.CreateResult<IEnumerable<ArticleViewModel>>();
            var (statusResponse, notFoundResponse) =
                await _requestClientArticlesByIdAuthor.GetResponse<ArticlesByIdAuthorResponseDto, NoArticlesFoundForAuthor>(new ArticlesByIdAuthorRequestDto //use Ex NoArticlesFound
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
                await _requestClientArticleById.GetResponse<ArticleResponseDto, NoArticlesFoundForAuthor>(new ArticleByIdRequestDto //use Ex NoArticlesFound
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
                await _requestClientArticles.GetResponse<ArticlesResponseDto, NoArticlesFoundForAuthor>(new ArticlesRequestDto()); //use Ex NoArticlesFound
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = _mapper.Map<IEnumerable<ArticleViewModel>>(statusResponse.Result.Message.ArticlesDto);
                return Ok(operation);
            }
            operation.AddError(new Exception(notFoundResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }
    }
}
