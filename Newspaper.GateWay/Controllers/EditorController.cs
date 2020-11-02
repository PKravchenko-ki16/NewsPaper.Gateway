using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Calabonga.OperationResults;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newspaper.GateWay.ViewModels.ViewModels.Editor;
using NewsPaper.MassTransit.Contracts.DTO.Exception.Editor;
using NewsPaper.MassTransit.Contracts.DTO.Requests.Editor;
using NewsPaper.MassTransit.Contracts.DTO.Responses.Editor;
using Newtonsoft.Json;

namespace NewsPaper.GateWay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditorController : ControllerBase
    {
        private readonly IRequestClient<EditorRequestDto> _requestClientEditor;
        private readonly IRequestClient<EditorsRequestDto> _requestClientEditors;
        private readonly IRequestClient<GuidEditorRequestDto> _requestClientGuidEditor;
        private readonly IRequestClient<NikeNameEditorRequestDto> _requestClientNikeNameEditor;
        private readonly IMapper _mapper;

        public EditorController(IMapper mapper,
            IRequestClient<EditorRequestDto> requestClientEditor,
            IRequestClient<EditorsRequestDto> requestClientEditors,
            IRequestClient<GuidEditorRequestDto> requestClientGuidEditor,
            IRequestClient<NikeNameEditorRequestDto> requestClientNikeNameEditor)
        {
            _mapper = mapper;
            _requestClientEditor = requestClientEditor;
            _requestClientEditors = requestClientEditors;
            _requestClientGuidEditor = requestClientGuidEditor;
            _requestClientNikeNameEditor = requestClientNikeNameEditor;
        }

        [Authorize]
        [HttpGet("getbyideditor")]
        public async Task<IActionResult> GetByIdEditor(Guid editorGuid)
        {
            var operation = OperationResult.CreateResult<EditorViewModel>();
            var (statusResponse, notFoundResponse) =
                await _requestClientEditor.GetResponse<EditorResponseDto, NoEditorFound>(new EditorRequestDto
                {
                    EditorGuid = editorGuid
                });
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = _mapper.Map<EditorViewModel>(statusResponse.Result.Message.EditorDto);
                return Ok(operation);
            }
            operation.AddError(new Exception(notFoundResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }

        [Authorize]
        [HttpGet("geteditors")]
        public async Task<IActionResult> GetEditors()
        {
            var operation = OperationResult.CreateResult<IEnumerable<EditorViewModel>>();
            var (statusResponse, notFoundResponse) =
                await _requestClientEditors.GetResponse<EditorsResponseDto, NoEditorFound>(new EditorsRequestDto());
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = _mapper.Map<IEnumerable<EditorViewModel>>(statusResponse.Result.Message.EditorsDto);
                return Ok(operation);
            }
            operation.AddError(new Exception(notFoundResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }

        [Authorize]
        [HttpGet("getguideditor")]
        public async Task<IActionResult> GetGuidEditor(string nikeNameEditor)
        {
            var operation = OperationResult.CreateResult<Guid>();
            var (statusResponse, notFoundResponse) =
                await _requestClientGuidEditor.GetResponse<GuidEditorResponseDto, NoEditorFound>(new GuidEditorRequestDto { NikeNameEditor = nikeNameEditor });
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = statusResponse.Result.Message.EditorGuid;
                return Ok(operation);
            }
            operation.AddError(new Exception(notFoundResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }

        [Authorize]
        [HttpGet("getnikenameeditor")]
        public async Task<IActionResult> GetNikeNameAuthor(Guid editorGuid)
        {
            var operation = OperationResult.CreateResult<string>();
            var (statusResponse, notFoundResponse) =
                await _requestClientNikeNameEditor.GetResponse<NikeNameEditorResponseDto, NoEditorFound>(new NikeNameEditorRequestDto { EditorGuid = editorGuid });
            if (statusResponse.IsCompletedSuccessfully)
            {
                operation.Result = statusResponse.Result.Message.NikeNameEditor;
                return Ok(operation);
            }
            operation.AddError(new Exception(notFoundResponse.Result.Message.MassageException));
            var output = JsonConvert.SerializeObject(operation);

            return Ok(output);
        }
    }
}
