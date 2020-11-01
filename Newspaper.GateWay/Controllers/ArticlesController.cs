using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Calabonga.OperationResults;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newspaper.GateWay.ViewModels.ViewModels;
using NewsPaper.MassTransit.Contracts.DTO.Exception.Articles;
using NewsPaper.MassTransit.Contracts.DTO.Requests.Articles;
using NewsPaper.MassTransit.Contracts.DTO.Responses.Articles;
using Newtonsoft.Json;

namespace NewsPaper.GateWay.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IRequestClient<ArticlesByIdAuthorRequestDto> _requestClient;
        private readonly IMapper _mapper;
        public ArticlesController(IMapper mapper,IRequestClient<ArticlesByIdAuthorRequestDto> requestClient)
        {
            _mapper = mapper;
            _requestClient = requestClient;
        }

        [Authorize]
        [HttpGet("getarticlesbyauthor")]
        public async Task<IActionResult> GetArticlesByAuthor(Guid authorGuid)
        {
            var operation = OperationResult.CreateResult<IEnumerable<ArticleViewModelApi>>();
            var (statusResponse, notFoundResponse) =
                await _requestClient.GetResponse<ArticlesResponseDto, NoArticlesFoundForAuthor>(new ArticlesByIdAuthorRequestDto
                {
                    AuthorGuid = authorGuid
                });
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = _mapper.Map<IEnumerable<ArticleViewModelApi>>(statusResponse.Result.Message.ArticlesDto);
                return Ok(operation);
            }
            operation.AddError( new Exception(notFoundResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }
    }
}
