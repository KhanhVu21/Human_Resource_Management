using HR.Application.Dto.Dto;
using HR.Application.Interface;
using HR.Domain.Kernel.Utils;
using HR.Infrastructure.Validation.Models.BusinessTrip;
using HR.Infrastructure.WebApi.Permission;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/businessTrip")]
public class BusinessTripController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BusinessTripController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public BusinessTripController(IUnitOfWork unitOfWork, ILogger<BusinessTripController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ BusinessTripController ]==============================================

    // GET: api/BusinessTrip/getListBusinessTripByHistory
    [HttpGet("getListBusinessTripByHistory")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListBusinessTripByHistory(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.BusinessTrip.GetBusinessTripAndWorkFlowByIdUserApproved(idUserCurrent, pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/BusinessTrip/getListBusinessTripByRole
    [HttpGet("getListBusinessTripByRole")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListBusinessTripByRole(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.BusinessTrip.GetBusinessTripAndWorkFlow(idUserCurrent, pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/BusinessTripModel/getListBusinessTrip
    [HttpGet("getListBusinessTrip")]
    public async Task<IActionResult> GetListBusinessTrip(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.BusinessTrip.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/BusinessTripModel/getBusinessTripById
    [HttpGet("getBusinessTripById")]
    public async Task<IActionResult> GetBusinessTripById(Guid idBusinessTrip)
    {
        var templateApi = await _unitOfWork.BusinessTrip.GetById(idBusinessTrip);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/BusinessTripModel/insertBusinessTrip
    [HttpPost("insertBusinessTrip")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> InsertBusinessTrip(BusinessTripModel businessTripModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var businessTripDto = businessTripModel.Adapt<BusinessTripDto>();

        businessTripDto.Id = Guid.NewGuid();
        businessTripDto.CreatedDate = DateTime.Now;
        businessTripDto.Status = 0;
        businessTripDto.IdUserRequest = idUserCurrent;

        var codeWorkFlow = AppSettings.BusinessTrip;
        var result =
            await _unitOfWork.WorkFlow.InsertStepWorkFlow(codeWorkFlow, businessTripDto.Id,
                idUserCurrent,
                nameUserCurrent);

        if (result.Success)
        {
            await _unitOfWork.BusinessTrip.Insert(businessTripDto, idUserCurrent, nameUserCurrent);
        }

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/BusinessTripModel/updateBusinessTrip
    [HttpPut("updateBusinessTrip")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> UpdateBusinessTrip(BusinessTripModel businessTripModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var businessTripDto = businessTripModel.Adapt<BusinessTripDto>();

        var result = await _unitOfWork.BusinessTrip.Update(businessTripDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/BusinessTripModel/deleteBusinessTrip
    [HttpDelete("deleteBusinessTrip")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> DeleteBusinessTrip(List<Guid> idBusinessTrip)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result = await _unitOfWork.BusinessTrip.RemoveByList(idBusinessTrip, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}