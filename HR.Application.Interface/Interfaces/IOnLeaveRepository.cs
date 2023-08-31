using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface IOnLeaveRepository : IRepository<OnLeaveDto>
{
    Task<TemplateApi<OnLeaveDto>> GetOnLeaveAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize);
    Task<TemplateApi<OnLeaveDto>> GetOnLeaveAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber, int pageSize);
}