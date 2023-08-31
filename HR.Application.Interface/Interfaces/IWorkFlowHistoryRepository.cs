using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface IWorkFlowHistoryRepository
{
    Task<TemplateApi<WorkflowHistoryDto>> GetWorkFlowHistoryByIdInstance(Guid idWorkFLowInstance, int pageNumber,
        int pageSize);
}