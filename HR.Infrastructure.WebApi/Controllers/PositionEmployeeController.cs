using HR.Application.Dto.Dto;
using HR.Application.Interface;
using HR.Domain.Kernel.Utils;
using HR.Infrastructure.Validation.Models.CategoryCity;
using HR.Infrastructure.Validation.Models.PositionEmployee;
using HR.Infrastructure.WebApi.Permission;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/positionEmployee")]
public class PositionEmployeeController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PositionEmployeeController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public PositionEmployeeController(IUnitOfWork unitOfWork, ILogger<PositionEmployeeController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ PositionEmployeeController ]==============================================

    // GET: api/PositionEmployee/getListPositionEmployee
    [HttpGet("getListPositionEmployee")]
    public async Task<IActionResult> GetListPositionEmployee(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.PositionEmployee.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/PositionEmployee/getPositionEmployeeById
    [HttpGet("getPositionEmployeeById")]
    public async Task<IActionResult> GetPositionEmployeeById(Guid idPositionEmployee)
    {
        var templateApi = await _unitOfWork.PositionEmployee.GetById(idPositionEmployee);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/PositionEmployee/insertPositionEmployee
    [HttpPost("insertPositionEmployee")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertPositionEmployee(PositionEmployeeModel positionEmployeeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var positionEmployeeDto = positionEmployeeModel.Adapt<PositionEmployeeDto>();

        positionEmployeeDto.Id = Guid.NewGuid();
        positionEmployeeDto.CreatedDate = DateTime.Now;
        positionEmployeeDto.Status = 0;

        var result = await _unitOfWork.PositionEmployee.Insert(positionEmployeeDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/PositionEmployee/updatePositionEmployee
    [HttpPut("updatePositionEmployee")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdatePositionEmployee(PositionEmployeeModel positionEmployeeModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var positionEmployeeDto = positionEmployeeModel.Adapt<PositionEmployeeDto>();

        var result = await _unitOfWork.PositionEmployee.Update(positionEmployeeDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/PositionEmployee/deletePositionEmployee
    [HttpDelete("deletePositionEmployee")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeletePositionEmployee(List<Guid> idPositionEmployee)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.PositionEmployee.RemoveByList(idPositionEmployee, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}