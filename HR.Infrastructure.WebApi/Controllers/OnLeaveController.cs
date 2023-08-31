using HR.Application.Dto.Dto;
using HR.Application.Interface;
using HR.Domain.Kernel.Utils;
using HR.Infrastructure.Validation.Models.CategoryCity;
using HR.Infrastructure.Validation.Models.OnLeave;
using HR.Infrastructure.WebApi.Permission;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/onLeave")]
public class OnLeaveController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OnLeaveController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public OnLeaveController(IUnitOfWork unitOfWork, ILogger<OnLeaveController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ OnLeaveController ]==============================================

    // GET: api/OnLeave/getListOnLeaveByHistory
    [HttpGet("getListOnLeaveByHistory")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListOnLeaveByHistory(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.OnLeave.GetOnLeaveAndWorkFlowByIdUserApproved(idUserCurrent, pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/OnLeave/getListOnLeaveByRole
    [HttpGet("getListOnLeaveByRole")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListOnLeaveByRole(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.OnLeave.GetOnLeaveAndWorkFlow(idUserCurrent, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/OnLeave/getListOnLeave
    [HttpGet("getListOnLeave")]
    public async Task<IActionResult> GetListOnLeave(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.OnLeave.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/OnLeave/getOnLeaveById
    [HttpGet("getOnLeaveById")]
    public async Task<IActionResult> GetOnLeaveById(Guid idOnLeave)
    {
        var templateApi = await _unitOfWork.OnLeave.GetById(idOnLeave);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/OnLeave/insertOnLeave
    [HttpPost("insertOnLeave")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertOnLeave(OnLeaveModel onLeaveModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var onLeaveDto = onLeaveModel.Adapt<OnLeaveDto>();

        onLeaveDto.Id = Guid.NewGuid();
        onLeaveDto.CreatedDate = DateTime.Now;
        onLeaveDto.Status = 0;
        onLeaveDto.IdUserRequest = idUserCurrent;

        var codeWorkFlow = AppSettings.OnLeave;
        var result =
            await _unitOfWork.WorkFlow.InsertStepWorkFlow(codeWorkFlow, onLeaveDto.Id,
                idUserCurrent,
                nameUserCurrent);

        if (result.Success)
        {
            await _unitOfWork.OnLeave.Insert(onLeaveDto, idUserCurrent, nameUserCurrent);
        }

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/OnLeave/updateOnLeave
    [HttpPut("updateOnLeave")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateOnLeave(OnLeaveModel onLeaveModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var onLeaveDto = onLeaveModel.Adapt<OnLeaveDto>();

        var result = await _unitOfWork.OnLeave.Update(onLeaveDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/OnLeave/deleteOnLeave
    [HttpDelete("deleteOnLeave")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteOnLeave(List<Guid> idOnLeave)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.OnLeave.RemoveByList(idOnLeave, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}