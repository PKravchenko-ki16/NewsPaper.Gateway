using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newspaper.GateWay.ViewModels.ViewModels;
using NewsPaper.MassTransit.Contracts.DTO.Requests;
using NewsPaper.MassTransit.Contracts.DTO.Responses;

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
        public async Task<ActionResult> GetArticlesByAuthor(Guid authorGuid)
        {
            var articles = await _requestClient.GetResponse<ArticlesResponseDto>(new ArticlesByIdAuthorRequestDto
            {
                AuthorGuid = authorGuid
            });
            var model = _mapper.Map<IEnumerable<ArticleViewModelApi>>(articles.Message.ArticlesDto);
            return Ok(model);
        }
    }
}
