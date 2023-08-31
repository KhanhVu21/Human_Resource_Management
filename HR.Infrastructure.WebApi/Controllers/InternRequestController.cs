using HR.Application.Dto.Dto;
using HR.Application.Interface;
using HR.Domain.Kernel.Utils;
using HR.Infrastructure.Validation.Models.CategoryCity;
using HR.Infrastructure.Validation.Models.InternRequest;
using HR.Infrastructure.WebApi.Permission;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/internRequest")]
public class InternRequestController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<InternRequestController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public InternRequestController(IUnitOfWork unitOfWork, ILogger<InternRequestController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ InternRequestController ]==============================================

    // GET: api/InternRequest/getListInternRequestByHistory
    [HttpGet("getListInternRequestByHistory")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListInternRequestByHistory(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.InternRequest.GetInternRequestAndWorkFlowByIdUserApproved(idUserCurrent, pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/InternRequest/getListInternRequestByRole
    [HttpGet("getListInternRequestByRole")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListInternRequestByRole(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.InternRequest.GetInternRequestAndWorkFlow(idUserCurrent, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/InternRequest/getListInternRequest
    [HttpGet("getListInternRequest")]
    public async Task<IActionResult> GetListInternRequest(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.InternRequest.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/InternRequest/getInternRequestById
    [HttpGet("getInternRequestById")]
    public async Task<IActionResult> GetInternRequestById(Guid idInternRequest)
    {
        var templateApi = await _unitOfWork.InternRequest.GetById(idInternRequest);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/InternRequest/insertInternRequest
    [HttpPost("insertInternRequest")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertInternRequest(InternRequestModel internRequestModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var internRequestDto = internRequestModel.Adapt<InternRequestDto>();

        internRequestDto.Id = Guid.NewGuid();
        internRequestDto.CreatedDate = DateTime.Now;
        internRequestDto.Status = 0;
        internRequestDto.IdUserRequest = idUserCurrent;

        var codeWorkFlow = AppSettings.InternRequest;
        var result =
            await _unitOfWork.WorkFlow.InsertStepWorkFlow(codeWorkFlow, internRequestDto.Id,
                idUserCurrent,
                nameUserCurrent);

        if (result.Success)
        {
            await _unitOfWork.InternRequest.Insert(internRequestDto, idUserCurrent, nameUserCurrent);
        }

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/InternRequest/updateInternRequest
    [HttpPut("updateInternRequest")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateInternRequest(InternRequestModel internRequestModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var internRequestDto = internRequestModel.Adapt<InternRequestDto>();

        var result = await _unitOfWork.InternRequest.Update(internRequestDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/InternRequest/deleteInternRequest
    [HttpDelete("deleteInternRequest")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteInternRequest(List<Guid> idInternRequest)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.InternRequest.RemoveByList(idInternRequest, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}