using HR.Application.Dto.Dto;
using HR.Application.Interface;
using HR.Domain.Kernel.Utils;
using HR.Infrastructure.Validation.Models.CategoryCity;
using HR.Infrastructure.Validation.Models.Overtime;
using HR.Infrastructure.WebApi.Permission;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/overtime")]
public class OvertimeController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OvertimeController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public OvertimeController(IUnitOfWork unitOfWork, ILogger<OvertimeController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ OvertimeController ]==============================================

    // GET: api/Overtime/getListOvertimeByHistory
    [HttpGet("getListOvertimeByHistory")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListOvertimeByHistory(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.Overtime.GetOvertimeDAndWorkFlowByIdUserApproved(idUserCurrent, pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Overtime/getListOvertimeByRole
    [HttpGet("getListOvertimeByRole")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListOvertimeByRole(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.Overtime.GetOvertimeAndWorkFlow(idUserCurrent, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Overtime/getListOvertime
    [HttpGet("getListOvertime")]
    public async Task<IActionResult> GetListOvertime(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.Overtime.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/Overtime/getOvertimeById
    [HttpGet("getOvertimeById")]
    public async Task<IActionResult> GetOvertimeById(Guid idOvertime)
    {
        var templateApi = await _unitOfWork.Overtime.GetById(idOvertime);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/Overtime/insertOvertime
    [HttpPost("insertOvertime")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertOvertime(OvertimeModel overtimeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var overtimeDto = overtimeModel.Adapt<OvertimeDto>();

        overtimeDto.Id = Guid.NewGuid();
        overtimeDto.CreatedDate = DateTime.Now;
        overtimeDto.Status = 0;
        overtimeDto.IdUserRequest = idUserCurrent;

        var codeWorkFlow = AppSettings.Overtime;
        var result =
            await _unitOfWork.WorkFlow.InsertStepWorkFlow(codeWorkFlow, overtimeDto.Id,
                idUserCurrent,
                nameUserCurrent);

        if (result.Success)
        {
            await _unitOfWork.Overtime.Insert(overtimeDto, idUserCurrent, nameUserCurrent);
        }

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/Overtime/updateOvertime
    [HttpPut("updateOvertime")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateOvertime(OvertimeModel overtimeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var overtimeDto = overtimeModel.Adapt<OvertimeDto>();

        var result = await _unitOfWork.Overtime.Update(overtimeDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/Overtime/deleteOvertime
    [HttpDelete("deleteOvertime")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteOvertime(List<Guid> idOvertime)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.Overtime.RemoveByList(idOvertime, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}