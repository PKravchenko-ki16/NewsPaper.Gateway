using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Calabonga.OperationResults;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newspaper.GateWay.ViewModels.ViewModels.User;
using NewsPaper.MassTransit.Contracts.DTO.Exception.User;
using NewsPaper.MassTransit.Contracts.DTO.Requests.User;
using NewsPaper.MassTransit.Contracts.DTO.Responses.User;
using Newtonsoft.Json;

namespace NewsPaper.GateWay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRequestClient<UserRequestDto> _requestClientUser;
        private readonly IRequestClient<UsersRequestDto> _requestClientUsers;
        private readonly IRequestClient<GuidUserRequestDto> _requestClientGuidUser;
        private readonly IRequestClient<NikeNameUserRequestDto> _requestClientNikeNameUser;
        private readonly IMapper _mapper;

        public UserController(IMapper mapper,
            IRequestClient<UserRequestDto> requestClientUser,
            IRequestClient<UsersRequestDto> requestClientUsers,
            IRequestClient<GuidUserRequestDto> requestClientGuidUser,
            IRequestClient<NikeNameUserRequestDto> requestClientNikeNameUser)
        {
            _mapper = mapper;
            _requestClientUser = requestClientUser;
            _requestClientUsers = requestClientUsers;
            _requestClientGuidUser = requestClientGuidUser;
            _requestClientNikeNameUser = requestClientNikeNameUser;
        }

        [Authorize]
        [HttpGet("getbyiduser")]
        public async Task<IActionResult> GetByIdUser(Guid userGuid)
        {
            var operation = OperationResult.CreateResult<UserViewModel>();
            var (statusResponse, notFoundResponse) =
                await _requestClientUser.GetResponse<UserResponseDto, NoUserFound>(new UserRequestDto
                {
                    UserGuid = userGuid
                });
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = _mapper.Map<UserViewModel>(statusResponse.Result.Message.UserDto);
                return Ok(operation);
            }
            operation.AddError(new Exception(notFoundResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }

        [Authorize]
        [HttpGet("getusers")]
        public async Task<IActionResult> GetUsers()
        {
            var operation = OperationResult.CreateResult<IEnumerable<UserViewModel>>();
            var (statusResponse, notFoundResponse) =
                await _requestClientUsers.GetResponse<UsersResponseDto, NoUserFound>(new UsersRequestDto());
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = _mapper.Map<IEnumerable<UserViewModel>>(statusResponse.Result.Message.UsersDto);
                return Ok(operation);
            }
            operation.AddError(new Exception(notFoundResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }

        [Authorize]
        [HttpGet("getguiduser")]
        public async Task<IActionResult> GetGuidUser(string nikeNameUser)
        {
            var operation = OperationResult.CreateResult<Guid>();
            var (statusResponse, notFoundResponse) =
                await _requestClientGuidUser.GetResponse<GuidUserResponseDto, NoUserFound>(new GuidUserRequestDto { NikeNameUser = nikeNameUser });
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = statusResponse.Result.Message.UserGuid;
                return Ok(operation);
            }
            operation.AddError(new Exception(notFoundResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }

        [Authorize]
        [HttpGet("getnikenameuser")]
        public async Task<IActionResult> GetNikeNameUser(Guid userGuid)
        {
            var operation = OperationResult.CreateResult<string>();
            var (statusResponse, notFoundResponse) =
                await _requestClientNikeNameUser.GetResponse<NikeNameUserResponseDto, NoUserFound>(new NikeNameUserRequestDto { UserGuid = userGuid });
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = statusResponse.Result.Message.NikeNameUser;
                return Ok(operation);
            }
            operation.AddError(new Exception(notFoundResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }
    }
}
