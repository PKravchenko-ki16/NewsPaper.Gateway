using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newspaper.GateWay.ViewModels.ViewModels;
using NewsPaper.MassTransit.Contracts.DTO.Exception.Articles;
using NewsPaper.MassTransit.Contracts.DTO.Requests.Articles;
using NewsPaper.MassTransit.Contracts.DTO.Responses.Articles;

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
            var (statusResponse, notFoundResponse) =
                await _requestClient.GetResponse<ArticlesResponseDto, NoArticlesFoundForAuthor>(new ArticlesByIdAuthorRequestDto
            {
                AuthorGuid = authorGuid
            });
            
            if (statusResponse.IsCompletedSuccessfully)
            {
                var articles = await statusResponse;
                var models = _mapper.Map<IEnumerable<ArticleViewModelApi>>(articles.Message.ArticlesDto);
                return Ok(models);
            }
            else
            {
                var notFound = await notFoundResponse;
                return Ok(notFound.Message);
            }
        }
    }
}
