using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface IWorkflowStepRepository : IRepository<WorkflowStepDto>
{
    Task<TemplateApi<WorkflowStepDto>> GetAllByIdTemplate(int pageNumber, int pageSize, Guid idTemplate);

    Task<TemplateApi<WorkflowStepDto>> CUD_WorkflowStep(List<WorkflowStepDto> workflowStepDto, Guid idUserCurrent,
        string fullName);
}