using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface IWorkFlowRepository
{
    Task<TemplateApi<WorkflowTemplateDto>> UpdateStepWorkFlow(Guid idWorkFlowInstance, bool isTerminated,
        bool isRequestToChange, string message, Guid idUserCurrent,
        string fullName);

    Task<TemplateApi<WorkflowTemplateDto>> InsertStepWorkFlow(string codeWorkFlow, Guid itemId, Guid idUserCurrent,
        string fullName);
}