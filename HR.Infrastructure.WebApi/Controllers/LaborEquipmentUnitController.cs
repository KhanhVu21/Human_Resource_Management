using HR.Application.Dto.Dto;
using HR.Application.Interface;
using HR.Domain.Kernel.Utils;
using HR.Infrastructure.Validation.Models.CategoryCity;
using HR.Infrastructure.WebApi.Permission;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/LaborEquipmentUnit")]
public class LaborEquipmentUnitController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LaborEquipmentUnitController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public LaborEquipmentUnitController(IUnitOfWork unitOfWork, ILogger<LaborEquipmentUnitController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ LaborEquipmentUnitController ]==============================================

    // HttpPut: api/LaborEquipmentUnit/updateLaborEquipmentUnitByListIdAndStatus
    [HttpPut("updateLaborEquipmentUnitByListIdAndStatus")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> UpdateLaborEquipmentUnitByListIdAndStatus(Guid idLaborEquipmentUnit, int status)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.LaborEquipmentUnit.UpdateLaborEquipmentUnitByListIdAndStatus(idLaborEquipmentUnit, status,
                idUserCurrent,
                nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // GET: api/LaborEquipmentUnit/getListLaborEquipmentUnitByStatus
    [HttpGet("getListLaborEquipmentUnitByStatus")]
    public async Task<IActionResult> GetListLaborEquipmentByStatus(int pageNumber, int pageSize, int status)
    {
        var templateApi =
            await _unitOfWork.LaborEquipmentUnit.GetLaborEquipmentUnitByStatus(status, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/LaborEquipmentUnit/getListLaborEquipmentUnitByUnitAndEmployee
    [HttpGet("getListLaborEquipmentUnitByUnitAndEmployee")]
    public async Task<IActionResult> GetListLaborEquipmentUnitByUnitAndEmployee(int pageNumber, int pageSize,
        Guid idUnit,
        Guid idEmployee)
    {
        var templateApi =
            await _unitOfWork.LaborEquipmentUnit.GetLaborEquipmentUnitByUnitAndEmployee(idUnit, idEmployee, pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/LaborEquipmentUnit/getListLaborEquipmentUnitByIdLaborEquipment
    [HttpGet("getListLaborEquipmentUnitByIdLaborEquipment")]
    public async Task<IActionResult> GetListLaborEquipmentUnitByIdLaborEquipment(int pageNumber, int pageSize,
        Guid idTicketLaborEquipment)
    {
        var templateApi =
            await _unitOfWork.LaborEquipmentUnit.GetLaborEquipmentUnitByIdTicketLaborEquipment(idTicketLaborEquipment,
                pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/LaborEquipmentUnit/getListLaborEquipmentUnitByIdUnit
    [HttpGet("getListLaborEquipmentUnitByIdUnit")]
    public async Task<IActionResult> GetListLaborEquipmentUnitByIdUnit(int pageNumber, int pageSize, Guid idUnit)
    {
        var templateApi =
            await _unitOfWork.LaborEquipmentUnit.GetLaborEquipmentUnitByUnit(idUnit, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/LaborEquipmentUnit/getListLaborEquipmentUnit
    [HttpGet("getListLaborEquipmentUnit")]
    public async Task<IActionResult> GetListLaborEquipmentUnit(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.LaborEquipmentUnit.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/LaborEquipmentUnit/getLaborEquipmentUnitById
    [HttpGet("getLaborEquipmentUnitById")]
    public async Task<IActionResult> GetLaborEquipmentUnitById(Guid idLaborEquipmentUnit)
    {
        var templateApi = await _unitOfWork.LaborEquipmentUnit.GetById(idLaborEquipmentUnit);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/LaborEquipmentUnit/insertLaborEquipmentUnit
    [Authorize(ListRole.Admin)]
    [HttpPost("insertLaborEquipmentUnit")]
    public async Task<IActionResult> InsertLaborEquipmentUnit(Guid idTicketLaborEquipment)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.LaborEquipmentUnit.InsertLaborEquipmentUnitTypeInsert(
                idTicketLaborEquipment, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/LaborEquipmentUnit/updateStatusLaborEquipmentUnit
    [HttpPut("updateStatusLaborEquipmentUnit")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateStatusLaborEquipmentUnit(Guid idLaborEquipmentUnit, int status)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.LaborEquipmentUnit.UpdateStatusLaborEquipmentUnit(idLaborEquipmentUnit, status,
                idUserCurrent,
                nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    #endregion
}