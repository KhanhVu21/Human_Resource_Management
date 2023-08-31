using HR.Application.Dto.Dto;
using HR.Application.Interface;
using HR.Domain.Kernel.Utils;
using HR.Infrastructure.Validation.Models.CategoryCity;
using HR.Infrastructure.Validation.Models.PromotionTranfer;
using HR.Infrastructure.WebApi.Permission;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/promotionTranfer")]
public class PromotionTranferController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PromotionTranferController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public PromotionTranferController(IUnitOfWork unitOfWork, ILogger<PromotionTranferController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ PromotionTranferController ]==============================================

    // GET: api/PromotionTranfer/getListPromotionTranferByHistory
    [HttpGet("getListPromotionTranferByHistory")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListPromotionTranferByHistory(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.PromotionTranfer.GetPromotionTranferAndWorkFlowByIdUserApproved(idUserCurrent,
                pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/PromotionTranfer/getListPromotionTranferByRole
    [HttpGet("getListPromotionTranferByRole")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> GetListPromotionTranferByRole(int pageNumber, int pageSize)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;

        var templateApi =
            await _unitOfWork.PromotionTranfer.GetPromotionTranferAndWorkFlow(idUserCurrent, pageNumber,
                pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/PromotionTranfer/getListPromotionTranfer
    [HttpGet("getListPromotionTranfer")]
    public async Task<IActionResult> GetListPromotionTranfer(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.PromotionTranfer.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/PromotionTranfer/getPromotionTranferById
    [HttpGet("getPromotionTranferById")]
    public async Task<IActionResult> GetPromotionTranferById(Guid idPromotionTranfer)
    {
        var templateApi = await _unitOfWork.PromotionTranfer.GetById(idPromotionTranfer);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPost: /api/PromotionTranfer/insertPromotionTranfer
    [HttpPost("insertPromotionTranfer")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> InsertPromotionTranfer(PromotionTranferModel promotionTranferModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var promotionTranferDto = promotionTranferModel.Adapt<PromotionTranferDto>();

        promotionTranferDto.Id = Guid.NewGuid();
        promotionTranferDto.CreatedDate = DateTime.Now;
        promotionTranferDto.Status = 0;
        promotionTranferDto.IdUserRequest = idUserCurrent;

        var codeWorkFlow = AppSettings.PromotionTranfer;
        var result =
            await _unitOfWork.WorkFlow.InsertStepWorkFlow(codeWorkFlow, promotionTranferDto.Id,
                idUserCurrent,
                nameUserCurrent);

        if (result.Success)
        {
            await _unitOfWork.PromotionTranfer.Insert(promotionTranferDto, idUserCurrent, nameUserCurrent);
        }

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPut: api/PromotionTranfer/updatePromotionTranfer
    [HttpPut("updatePromotionTranfer")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> UpdatePromotionTranfer(PromotionTranferModel promotionTranferModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var promotionTranferDto = promotionTranferModel.Adapt<PromotionTranferDto>();

        var result = await _unitOfWork.PromotionTranfer.Update(promotionTranferDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/PromotionTranfer/deletePromotionTranfer
    [HttpDelete("deletePromotionTranfer")]
    [Authorize(ListRole.User)]
    public async Task<IActionResult> DeletePromotionTranfer(List<Guid> idPromotionTranfer)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.PromotionTranfer.RemoveByList(idPromotionTranfer, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}