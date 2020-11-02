using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Calabonga.OperationResults;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newspaper.GateWay.ViewModels.ViewModels.Author;
using NewsPaper.MassTransit.Contracts.DTO.Exception.Author;
using NewsPaper.MassTransit.Contracts.DTO.Requests.Author;
using NewsPaper.MassTransit.Contracts.DTO.Responses.Author;
using Newtonsoft.Json;

namespace NewsPaper.GateWay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IRequestClient<AuthorRequestDto> _requestClientAuthor;
        private readonly IRequestClient<AuthorsRequestDto> _requestClientAuthors;
        private readonly IRequestClient<GuidAuthorRequestDto> _requestClientGuidAuthor;
        private readonly IRequestClient<NikeNameAuthorRequestDto> _requestClientNikeNameAuthor;
        private readonly IMapper _mapper;

        public AuthorController(IMapper mapper, 
            IRequestClient<AuthorRequestDto> requestClientAuthor,
            IRequestClient<AuthorsRequestDto> requestClientAuthors, 
            IRequestClient<GuidAuthorRequestDto> requestClientGuidAuthor,
            IRequestClient<NikeNameAuthorRequestDto> requestClientNikeNameAuthor)
        {
            _mapper = mapper;
            _requestClientAuthor = requestClientAuthor;
            _requestClientAuthors = requestClientAuthors;
            _requestClientGuidAuthor = requestClientGuidAuthor;
            _requestClientNikeNameAuthor = requestClientNikeNameAuthor;
        }

        [Authorize]
        [HttpGet("getbyidauthor")]
        public async Task<IActionResult> GetByIdAuthor(Guid authorGuid)
        {
            var operation = OperationResult.CreateResult<AuthorViewModel>();
            var (statusResponse, notFoundResponse) =
                await _requestClientAuthor.GetResponse<AuthorResponseDto, NoAuthorFound>(new AuthorRequestDto
                {
                     AuthorGuid = authorGuid
                });
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = _mapper.Map<AuthorViewModel>(statusResponse.Result.Message.AuthorDto);
                return Ok(operation);
            }
            operation.AddError(new Exception(notFoundResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }

        [Authorize]
        [HttpGet("getauthors")]
        public async Task<IActionResult> GetAuthors()
        {
            var operation = OperationResult.CreateResult<IEnumerable<AuthorViewModel>>();
            var (statusResponse, notFoundResponse) =
                await _requestClientAuthors.GetResponse<AuthorsResponseDto, NoAuthorFound>(new AuthorsRequestDto());
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = _mapper.Map<IEnumerable<AuthorViewModel>>(statusResponse.Result.Message.AuthorsDto);
                return Ok(operation);
            }
            operation.AddError(new Exception(notFoundResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }

        [Authorize]
        [HttpGet("getguidauthor")]
        public async Task<IActionResult> GetGuidAuthor(string nikeNameAuthor)
        {
            var operation = OperationResult.CreateResult<Guid>();
            var (statusResponse, notFoundResponse) =
                await _requestClientGuidAuthor.GetResponse<GuidAuthorResponseDto, NoAuthorFound>(new GuidAuthorRequestDto{NikeNameAuthor = nikeNameAuthor });
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = statusResponse.Result.Message.AuthorGuid;
                return Ok(operation);
            }
            operation.AddError(new Exception(notFoundResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }

        [Authorize]
        [HttpGet("getnikenameauthor")]
        public async Task<IActionResult> GetNikeNameAuthor(Guid authorGuid)
        {
            var operation = OperationResult.CreateResult<string>();
            var (statusResponse, notFoundResponse) =
                await _requestClientNikeNameAuthor.GetResponse<NikeNameAuthorResponseDto, NoAuthorFound>(new NikeNameAuthorRequestDto { AuthorGuid = authorGuid });
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = statusResponse.Result.Message.NikeNameAuthor;
                return Ok(operation);
            }
            operation.AddError(new Exception(notFoundResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }
    }
}
