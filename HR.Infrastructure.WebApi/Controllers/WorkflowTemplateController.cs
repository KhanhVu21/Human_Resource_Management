using HR.Application.Dto.Dto;
using HR.Application.Interface;
using HR.Domain.Kernel.Utils;
using HR.Infrastructure.Validation.Models.CategoryCity;
using HR.Infrastructure.Validation.Models.TypeDayOff;
using HR.Infrastructure.Validation.Models.WorkflowTemplate;
using HR.Infrastructure.WebApi.Permission;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/workflowTemplate")]
public class WorkflowTemplateController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WorkflowTemplateController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public WorkflowTemplateController(IUnitOfWork unitOfWork, ILogger<WorkflowTemplateController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ WorkflowTemplateController ]==============================================

    // GET: api/WorkflowTemplate/getListWorkflowTemplate
    [HttpGet("getListWorkflowTemplate")]
    public async Task<IActionResult> GetListWorkflowTemplate(int pageNumber, int pageSize)
    {
        var templateApi = await _unitOfWork.WorkflowTemplate.GetAllAsync(pageNumber, pageSize);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // GET: api/WorkflowTemplate/getWorkflowTemplateById
    [HttpGet("getWorkflowTemplateById")]
    public async Task<IActionResult> GetWorkflowTemplateById(Guid idWorkflowTemplate)
    {
        var templateApi = await _unitOfWork.WorkflowTemplate.GetById(idWorkflowTemplate);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    // HttpPut: api/WorkflowTemplate/updateWorkflowTemplate
    [HttpPut("updateWorkflowTemplate")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> UpdateWorkflowTemplate(WorkflowTemplateModel workflowTemplateModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var workflowTemplateDto = workflowTemplateModel.Adapt<WorkflowTemplateDto>();

        var result = await _unitOfWork.WorkflowTemplate.Update(workflowTemplateDto, idUserCurrent, nameUserCurrent);
        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpPost: /api/WorkflowTemplate/insertWorkflowTemplate
    [HttpPost("insertWorkflowTemplate")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> InsertWorkflowTemplate(WorkflowTemplateModel workflowTemplateModel)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var workflowTemplateDto = workflowTemplateModel.Adapt<WorkflowTemplateDto>();

        workflowTemplateDto.Id = Guid.NewGuid();
        workflowTemplateDto.CreatedDate = DateTime.Now;
        workflowTemplateDto.Status = 0;

        var result = await _unitOfWork.WorkflowTemplate.Insert(workflowTemplateDto, idUserCurrent, nameUserCurrent);

        _logger.LogInformation("Thành công : {message}", result.Message);
        return Ok(result);
    }

    // HttpDelete: /api/WorkflowTemplate/deleteWorkflowTemplate
    [HttpDelete("deleteWorkflowTemplate")]
    [Authorize(ListRole.Admin)]
    public async Task<IActionResult> DeleteWorkflowTemplate(List<Guid> idWorkflowTemplate)
    {
        var idUserCurrent = (Guid)Request.HttpContext.Items["UserId"]!;
        var nameUserCurrent = (string)Request.HttpContext.Items["UserName"]!;

        var result =
            await _unitOfWork.WorkflowTemplate.RemoveByList(idWorkflowTemplate, idUserCurrent, nameUserCurrent);
        return Ok(result);
    }

    #endregion
}